using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element //: MonoBehaviour
{
    private float speed = 5f;

    public int type { get; set; } //определяет тип элемента: 0 - топор
                                  //                         1 - меч
                                  //                         2 - лук
                                  //                         3 - зелье итд.
                                  //                        -1 - уничтожен

    //позиция элемента на доске
    public int posX { get; set; }
    public int posY { get; set; }

    public bool isBlocked { get; set; } // элемент не должен двигаться если попал в матч 


    public GameObject piece;
    public SpriteRenderer spriteRenderer;

    public Element(GameObject _piece)
    {
        setPiece(_piece);
    }

    private Element(GameObject _element, SpriteRenderer _spriteRenderer)
    {
        piece = _element;
        spriteRenderer = _spriteRenderer;
    }

    public void setPiece(GameObject _piece)
    {
        piece = _piece;
        spriteRenderer = piece.GetComponent<SpriteRenderer>();
    }

    public void setElement(Element elem)
    {
        piece = elem.piece;
        type = (piece == null) ? 0 : elem.type;
        if (piece == null) return;
        spriteRenderer = elem.spriteRenderer;
    }

    public void MoveElementTo(Vector2 move)
    {
        piece.transform.position = Vector2.Lerp(piece.transform.position, move, Time.deltaTime * speed);
    }

    public void hardMoveElementTo(Vector2 move)
    {
        piece.transform.position = Vector2.MoveTowards(piece.transform.position, move, Time.deltaTime * speed);
    }

    //Создает и возвращает копию себя
    public Element getElement()
    {
        Element clone = new Element(this.piece, this.spriteRenderer);
        clone.type = this.type;

        return clone;
    }
}
