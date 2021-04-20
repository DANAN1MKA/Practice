using UnityEngine;


[CreateAssetMenu(menuName = "Characters/PlayerDefaultCoef")]
public class PlayerItems : ScriptableObject
{
    public GameObject itemListPrefab;

    public bool[] isItemBought;
    public ItemData[] itemData;
}

