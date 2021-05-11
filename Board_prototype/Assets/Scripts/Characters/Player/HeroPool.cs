using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Characters/HeroPool")]
public class HeroPool : ScriptableObject
{
    public GameObject[] heroPrefab;

    public string[] name;

    public Sprite[] portrait;

    public bool[] isBought;

    public int[] price;
    //TODO: зависит от того что делают герои
}
