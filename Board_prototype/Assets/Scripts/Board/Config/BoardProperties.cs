using UnityEngine;

[CreateAssetMenu(menuName = "Configuratins/BoardProperties")]
public class BoardProperties : ScriptableObject
{
    public int width;
    public int height;
    public float time;
    public float additionalTime;
    public Vector2 boardPosition;
    public Vector2 boardPositionFromResolution;

    public GameObject elementPrefab;
    public GameObject linePrefab;
    //public Material[] pool;
    public Sprite[] pool;

    //TODO: перенести в отдельный объект
    public GameObject characterPrefab;
    public Vector2 characterPosition;

    public float scale;
}