using UnityEngine;

[CreateAssetMenu(menuName = "Configuratins/BoardConfiruratin")]
public class BoardConfig : ScriptableObject
{
    public int width;
    public int height;
    public float time;
    public float additionalTime;
    public Vector2 boardPosition;

    public GameObject elementPrefab;
    public Material[] pool;
}