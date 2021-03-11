using UnityEngine;

[CreateAssetMenu(menuName = "Configuratins/BoardConfiruratin")]
public class BoardConfig : ScriptableObject
{
    public int width;
    public int height;
    public float time;
    public float additionalTime;
    public Vector2 boardPosition;
    public Vector2 boardPositionFromResolution;

    public GameObject elementPrefab;
    public Material[] pool;


    public float scale;
}