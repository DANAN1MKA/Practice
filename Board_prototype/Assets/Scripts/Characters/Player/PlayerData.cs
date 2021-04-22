using UnityEngine;

[CreateAssetMenu(menuName = "Characters/PlayerData")]
public class PlayerData : ScriptableObject
{
    public GameObject currentHeroPrefab;
    public System.UInt64 score;
    public System.UInt64 money;
}
