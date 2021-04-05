using UnityEngine;

[CreateAssetMenu(menuName = "Characters/PlayerData")]
public class PlayerData : ScriptableObject
{
    public GameObject currentHeroPrefab;
    public int score;
    public int money;
    public float coefficient;
}
