using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BossButton : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    [SerializeField] GameObject button;

    private void Awake()
    {
        signalBus.Subscribe<ShowBossButton>(show);
    }

    private void Start()
    {
        hide();
    }

    private void hide()
    {
        button.SetActive(false);
    }

    private void show()
    {
        button.SetActive(true);
    }

    public void goBossScene()
    {
        hide();
        signalBus.Fire<BossStartedSignal>();
        //TODO: переход на сцену с боссом
    }
}
