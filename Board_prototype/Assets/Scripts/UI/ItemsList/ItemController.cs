using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemController: MonoBehaviour
{
    public delegate void updateUICallback();
    public updateUICallback updateUIcb;


    private int ID;
    [SerializeField]private Image image;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemCharacteristics;
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;

    private ItemsDataSObj itemData;
    private PlayerData playerData;

    private void Awake()
    {
        //image = GetComponentInChildren<Image>();

        //itemName = transform.Find("ItemName").gameObject.GetComponent<Text>();
        //itemCharacteristics = transform.Find("ItemCharacteristics").gameObject.GetComponent<Text>();

        //button = GetComponentInChildren<Button>();
        //buttonText = button.GetComponentInChildren<Text>();
    }

    public void setupItem(int _ID, ItemsDataSObj _itemData, PlayerData _playerData)
    {
        ID = _ID;
        itemData = _itemData;
        playerData = _playerData;


        updateUI();
    }

    public void itemButtonPressed()
    {
        if(playerData.score >= itemData.baseCoast[ID])
        {
            if (!itemData.isBought[ID])
            {
                playerData.score -= itemData.baseCoast[ID];


                itemData.isBought[ID] = true;
                itemData.level[ID] = 1;
                itemData.baseCoast[ID] = (System.UInt64)(itemData.baseCoast[ID] * 1.07f);
            }
            else
            {
                playerData.score -= itemData.baseCoast[ID];

                itemData.level[ID]++;
                itemData.baseCoast[ID] = (System.UInt64)(itemData.baseCoast[ID] * 1.07f);
                itemData.baseGrowthRate[ID] = DefaultCoef.itemsData[ID].baseGrowthRate * (System.UInt64)itemData.level[ID];

            }
            updateUI();
            updateUIcb();
        }
    }

    public void updateUI()
    {
        itemName.text = itemData.itemName[ID];
        image.sprite = itemData.sprite[ID];
        string itemCharacteristic = "+" + itemData.baseGrowthRate[ID] + "\n" +
                                    "level:" + itemData.level[ID] + "\n" +
                                    "next level:+" + DefaultCoef.itemsData[ID].baseGrowthRate;
        itemCharacteristics.text = itemCharacteristic;
        buttonText.text = itemData.baseCoast[ID] + "\nочков";
    }
}
