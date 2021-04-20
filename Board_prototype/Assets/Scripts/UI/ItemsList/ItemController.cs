using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemController: MonoBehaviour
{
    public delegate void updateUICallback();
    public updateUICallback updateUIcb;


    private int ID;
    private Image image;
    private Text itemName;
    private Text itemCharacteristics;
    private Button button;
    private Text buttonText;

    private ItemData itemData;
    private PlayerData playerData;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();

        itemName = transform.Find("ItemName").gameObject.GetComponent<Text>();
        itemCharacteristics = transform.Find("ItemCharacteristics").gameObject.GetComponent<Text>();

        button = GetComponentInChildren<Button>();
        buttonText = button.GetComponentInChildren<Text>();
    }

    public void setupItem(int _ID, ItemData _itemData, PlayerData _playerData)
    {
        ID = _ID;
        itemData = _itemData;
        playerData = _playerData;


        updateUI();
    }

    public void itemButtonPressed()
    {
        if(playerData.score >= itemData.baseCoast)
        {
            if (!itemData.isBought)
            {
                playerData.score -= itemData.baseCoast;


                itemData.isBought = true;
                itemData.level = 1;
                itemData.baseCoast = (System.UInt64)(itemData.baseCoast * 1.07f);
            }
            else
            {
                playerData.score -= itemData.baseCoast;

                itemData.level++;
                itemData.baseCoast = (System.UInt64)(itemData.baseCoast * 1.07f);
                itemData.baseGrowthRate = DefaultCoef.itemsData[ID].baseGrowthRate * (System.UInt64)itemData.level;

                //TODO: otladka
                Debug.Log("DefaultCoef:" + DefaultCoef.itemsData[ID].baseGrowthRate + " level:" + itemData.level);
            }
            updateUI();
            updateUIcb();
        }
    }

    public void updateUI()
    {
        itemName.text = itemData.itemName;
        string itemCharacteristic = "+" + itemData.baseGrowthRate + "\n" +
                                    "level:" + itemData.level + "\n" +
                                    "next level:+" + DefaultCoef.itemsData[ID].baseGrowthRate;
        itemCharacteristics.text = itemCharacteristic;
        buttonText.text = itemData.baseCoast + "\nочков";
    }
}
