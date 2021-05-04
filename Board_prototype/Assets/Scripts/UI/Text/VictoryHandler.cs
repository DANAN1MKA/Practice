using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class VictoryHandler : MonoBehaviour, IDragHandler
{
    [Inject] private SignalBus signalBus;

    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject defeatText;
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject sceneChanger;
    private SceneChanger sceneChangerScript;

    private void Awake()
    {
        signalBus.Subscribe<VictorySignal>(handle);
    }

    private void Start()
    {
        sceneChangerScript = sceneChanger.GetComponent<SceneChanger>();
    }

    private void handle(VictorySignal signal)
    {
        if (signal.victory) showMessage(victoryText);
        else showMessage(defeatText);
        shadow.SetActive(true);
    }

    private void showMessage(GameObject message)
    {
        message.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        sceneChangerScript.ChangeScene("Items");
    }
}
