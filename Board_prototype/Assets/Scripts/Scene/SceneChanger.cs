using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


public class SceneChanger : MonoBehaviour
{
	[Inject] private PlayerData playerData;
	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void Exit()
	{
		Application.Quit();
	}
}
