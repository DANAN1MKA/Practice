using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/ItemsData")]
public class ItemsDataSObj : ScriptableObject
{
    public GameObject itemListPrefab;

    public string[] itemName;
    public System.UInt64[] baseCoast;
    public System.UInt64[] baseGrowthRate;
    public int[] level;

    public bool[] isBought;

    public Sprite[] sprite;
    
}
