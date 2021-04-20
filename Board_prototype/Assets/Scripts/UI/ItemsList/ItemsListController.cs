using UnityEngine;
using Zenject;

public class ItemsListController : MonoBehaviour
{
    [Inject] SignalBus signalBus;

    [Inject] private PlayerItems playerItems;
    [Inject] private PlayerData playerData;

    private ItemController[] items;
    //[Inject] private GameObject itemListPrefab;

    private void Start()
    {
        items = new ItemController[8];

        for(int i = 0; i < DefaultCoef.itemsData.Length; i++)
        {
            GameObject item = Instantiate(playerItems.itemListPrefab);

            item.transform.parent = transform;
            item.transform.localScale = new Vector3(1,1,1);

            items[i] = item.GetComponent<ItemController>();
            items[i].setupItem(i, playerItems.itemData[i], playerData);
            items[i].updateUIcb = updateUI;
        }
    }

    private void updateUI()
    {
        signalBus.Fire<UpdateTextUISignal>();
    }

}
