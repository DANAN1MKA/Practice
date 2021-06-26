using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleHTTP;
using Zenject;

public class ClickerClient : MonoBehaviour
{
	[Inject] private SignalBus signalBus;
	//[Inject] private BoardProperties config;

	//private string validURL = "https://jsonplaceholder.typicode.com/posts/";
	private string validURL = "https://b1eca428-7c06-4525-8270-10d634238f49.mock.pstmn.io";


	private string gameID;
	private string playerID;

	private string findGameGet = "game-keys";

	//	get = game-keys - Find Game
	//	get = 2 - Opponent's move




	private void Awake()
    {
		signalBus.Subscribe<ClientReplaySignal>(postNewReplay);
		

	}


    public void postNewReplay(ClientReplaySignal signal)
    {
        SetStepJSON json = signal.json;
        json.gameID = gameID;
        json.playerID = playerID;

        StartCoroutine(playerMovePost(json));
    }



    IEnumerator FindNewGameGet(string baseUrl)
	{
		Request request = new Request(baseUrl + findGameGet);

		Client http = new Client();
		yield return http.Send(request);
		findGameResult(http);
	}

	private void findGameResult(Client http)
    {
		if (http.IsSuccessful())
		{
			Response resp = http.Response();
			FindGameData newGame = JsonUtility.FromJson<FindGameData>(resp.Body());
			gameID = newGame.gameID;
			playerID = newGame.playerID;
		}
	}

	IEnumerator opponentsMoveGet(string baseUrl)
	{
		Request request = new Request(baseUrl + "2");

		Client http = new Client();
		yield return http.Send(request);
		opponentsMoveResult(http);
	}
	private void opponentsMoveResult(Client http)
    {
		if (http.IsSuccessful())
		{
			Response resp = http.Response();
			SetStepJSON newReplay = JsonUtility.FromJson<SetStepJSON>(resp.Body());

			//TODO: сигнал на проигрыш реплея оппонента
			signalBus.Fire(new ServerReplaySignal(newReplay));
		}

	}

	IEnumerator playerMovePost(SetStepJSON json)
    {
		Request request = new Request(validURL)
			.Post(RequestBody.From<SetStepJSON>(json));

		Client http = new Client();
		yield return http.Send(request);
		playerMoveResult(http);
	}
	private void playerMoveResult(Client http)
    {
		if (http.IsSuccessful())
		{
			Response resp = http.Response();

			Debug.Log("status: " + resp.Status().ToString() + "\nbody: " + resp.Body());
		}
		else
		{
			Debug.Log("error: " + http.Error());
		}
	}
}


