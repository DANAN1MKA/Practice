using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HeroListController : MonoBehaviour
{
    [Inject] SignalBus signalBus;

    [Inject] private HeroPool heroes;
    [Inject] private PlayerData playerData;

    [SerializeField] GameObject itemListPrafab;

    private HeroItemController[] items;

    private void Start()
    {
        items = new HeroItemController[heroes.isBought.Length];

        for (int i = 0; i < heroes.isBought.Length; i++)
        {
            GameObject item = Instantiate(itemListPrafab);

            item.transform.parent = transform;
            item.transform.localScale = new Vector3(1, 1, 1);

            items[i] = item.GetComponent<HeroItemController>();
            items[i].setupItem(i, heroes, playerData, signalBus);
            items[i].updateUIcb = updateUI;
        }
    }

    private void updateUI()
    {
        for (int i = 0; i < items.Length; i++)
            items[i].updateUI();

        signalBus.Fire<UpdateTextUISignal>();
    }
}
