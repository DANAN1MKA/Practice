using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceGenerator_v2 : MonoBehaviour, IPieceGenerator
{
    [SerializeField] public GameObject elementPrefab;
    [SerializeField] public Material[] pool;


    public void changeType(Element element)
    {
        element.type = Random.Range(0, pool.Length);
        element.spriteRenderer.material = pool[element.type];

    }

    public Element generateCommonPiece()
    {
        GameObject newPiece = Instantiate(elementPrefab);
        Element newElement = new Element(newPiece);

        newElement.type = Random.Range(0, pool.Length);
        newElement.spriteRenderer.material = pool[newElement.type];

        return newElement;

    }

    public Element generateSpecialPiece()
    {
        return null;
    }

    public void setNewPiece(Element element)
    {
    }
}
