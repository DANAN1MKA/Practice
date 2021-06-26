using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleHTTP;
using Zenject;

public class MultyplayerService : MonoBehaviour
{
	[Inject] private SignalBus signalBus;
	[Inject] private PlayerData playerData;

	//private string validURL = "https://jsonplaceholder.typicode.com/posts/";
	//private string validURL = "https://b1eca428-7c06-4525-8270-10d634238f49.mock.pstmn.io";
	private string validURL = "http://157.230.113.81:8080/";
	//private string validURL = "http://10.0.2.2:8080/";


	private string gameID;
	private string playerID;

	private string findGameGet = "game-keys?heroId=";
	private string checkGameReady = "check-state";
	private string opponentsMove = "step?";
	private string movePost = "step";

	//	get = game-keys - Find Game
	//	get = check-state/{gameId} is game ready
	//	get = step?gameId=1&playerId=1 - Opponent's move


	public ClientReplaySignal lastPlayerReplay { get; private set; }

    public void findNewGame()
    {
		StartCoroutine(findNewGameGet(validURL));
    }
	public void checkGameStatus()
    {
		StartCoroutine(gameStatusGet(validURL));
	}
	public void postNewReplay(ClientReplaySignal signal)
	{
		lastPlayerReplay = signal;

		SetStepJSON json = signal.json;
		json.gameID = gameID;
		json.playerID = playerID;

		Debug.Log("gameID " + json.gameID);
		Debug.Log("playerID " + json.playerID);


		StartCoroutine(playerMovePost(json));
	}
	public void getOpponentReplay()
    {
		StartCoroutine(opponentsMoveGet(validURL));
    }


	//FindGameGET
	IEnumerator findNewGameGet(string baseUrl)
	{
		Request request = new Request(baseUrl + findGameGet + playerData.currentHeroID);


		//TODO: отладка
		Debug.Log("Find new game get URL--" + baseUrl + findGameGet + playerData.currentHeroID);


		Client http = new Client();
		yield return http.Send(request);
		findGameResult(http);
	}
	private void findGameResult(Client http)
	{
		if (http.IsSuccessful())
		{
			Response resp = http.Response();
			if (resp.Status() >= 200 && resp.Status() <= 300)
			{
				FindGameData newGame = JsonUtility.FromJson<FindGameData>(resp.Body());
				gameID = newGame.gameID;
				playerID = newGame.playerID;

				signalBus.Fire(new IsGameReadySignal(newGame.isGameReadyToStart, newGame.heroID));
			}
			//TODO: отладка
			Debug.Log("Find new game result status: " + resp.Status().ToString() + "\nbody: " + resp.Body());

		}
        else
        {
			signalBus.Fire<ServerNotRespondingSignal>();
		}
	}

	//FindGameStateGET
	IEnumerator gameStatusGet(string baseUrl)
	{
		//Request request = new Request(baseUrl + checkGameReady + "/" + gameID);
		Request request = new Request(baseUrl + checkGameReady + "/" + gameID + "?playerId=" + playerID);

		//TODO: отладка
		Debug.Log("new game check status URL--" + baseUrl + checkGameReady + "/" + gameID);

		Client http = new Client();
		yield return http.Send(request);
		gameStatusResult(http);

	}
	private void gameStatusResult(Client http)
    {
		if (http.IsSuccessful())
		{
			Response resp = http.Response();

			if (resp.Status() == 200)
			{
				GameStatusJSON newGame = JsonUtility.FromJson<GameStatusJSON>(resp.Body());

				signalBus.Fire(new IsGameReadySignal(newGame.status, newGame.heroID));
			}

			//TODO: отладка
			Debug.Log("new game check status result status: " + resp.Status().ToString() + "\nbody: " + resp.Body());
		}
		else
		{
			signalBus.Fire<ServerNotRespondingSignal>();
		}


	}

	//OpponentsMoveGET
	IEnumerator opponentsMoveGet(string baseUrl)
	{
		Request request = new Request(baseUrl + opponentsMove + "gameId=" + gameID + "&playerId=" + playerID);

		//TODO: отладка
		Debug.Log("Opponents move get URL--" + baseUrl + opponentsMove + "gameId=" + gameID + "&playerId=" + playerID);

		Client http = new Client();
		yield return http.Send(request);
		opponentsMoveResult(http);

	}
	private void opponentsMoveResult(Client http)
	{
		if (http.IsSuccessful())
		{
			Response resp = http.Response();

			if (resp.Status() >= 200 && resp.Status() <= 300)
			{
				SetStepJSON newReplay = JsonUtility.FromJson<SetStepJSON>(resp.Body());

				//TODO: сигнал на проигрыш реплея оппонента
				signalBus.Fire(new ServerReplaySignal(newReplay));
			}

			//TODO: отладка
			Debug.Log("Opponents move get result status: " + resp.Status().ToString() + "\nbody: " + resp.Body());
		}
		else
		{
			signalBus.Fire<ServerNotRespondingSignal>();
		}


	}

	//PlayerMovePOST 
	IEnumerator playerMovePost(SetStepJSON json)
	{
		Request request = new Request(validURL + movePost)
			.Post(RequestBody.From<SetStepJSON>(json));

		//TODO: отладка
		Debug.Log("Player move post URL-"+ validURL + movePost);


		Client http = new Client();
		yield return http.Send(request);
		playerMoveResult(http);
	}
	private void playerMoveResult(Client http) 
	{
		if (http.IsSuccessful())
		{
			Response resp = http.Response();

			//TODO: отладка
			Debug.Log("Player move result status: " + resp.Status().ToString() + "\nbody: " + resp.Body());

			if (resp.Status() == 200)
				signalBus.Fire(new ReplayReachedServerSignal(true));
			else 
				signalBus.Fire(new ReplayReachedServerSignal(false));

		}
		else
		{

			signalBus.Fire<ServerNotRespondingSignal>();

			Debug.Log("error: " + http.Error());
		}
	}

}
