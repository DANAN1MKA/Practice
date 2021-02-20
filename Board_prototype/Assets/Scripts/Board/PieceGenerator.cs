using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceGenerator : MonoBehaviour, IPieceGenerator
{
    [SerializeField] public GameObject elementPrefab;
    [SerializeField] public Sprite[] pool;

    public void changeType(Element element)
    {
        element.type = Random.Range(0, pool.Length);
        element.spriteRenderer.sprite = pool[element.type];
    }

    public Element generateCommonPiece()
    {
        GameObject newPiece = Instantiate(elementPrefab);
        Element newElement = new Element(newPiece);

        newElement.type = Random.Range(0, pool.Length);
        newElement.spriteRenderer.sprite = pool[newElement.type];

        return newElement;
    }

    public Element generateSpecialPiece()
    {
        return null;
    }

    public void setNewPiece(Element element)
    {
        //Destroy(element.piece);
        //GameObject newPiece = Instantiate(elementPrefab);

    }
}

public interface IPieceGenerator
{
    Element generateCommonPiece();

    void changeType(Element element);

    Element generateSpecialPiece();

    void setNewPiece(Element element);
}
