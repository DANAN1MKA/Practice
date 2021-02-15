using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Board : BoardFather
{
    [Inject] private Itimer timer;

    [Inject] private IPieceGenerator pieceGenerator;

    private Element[,] allElements;

    void Start()
    {
        initBoard();
    }

    public void initBoard()
    {
        allElements = new Element[width, heigth];

        Vector2 curentPiecePosition = new Vector2(_thisTransform.position.x, _thisTransform.position.y);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < heigth; j++)
            {
                Element newElement = pieceGenerator.generateCommonPiece();
                newElement.piece.transform.parent = _thisTransform;
                newElement.piece.transform.position = new Vector2(curentPiecePosition.x + i, curentPiecePosition.y + j);
                newElement.piece.name = "( " + i + "," + j + " )";
                newElement.posX = i;
                newElement.posY = j;

                do
                {
                    pieceGenerator.changeType(newElement);
                } while (isItInitMatch(newElement));

                allElements[i, j] = newElement;
            }
        }

    }
    private bool isItInitMatch(Element _elem)
    {
        bool isMatch = false;

        if (_elem.posX > 1 && _elem.posY > 1)
        {
            int i = 1;
            do
            {
                if (allElements[_elem.posX - i, _elem.posY].type == _elem.type ||
                    allElements[_elem.posX, _elem.posY - i].type == _elem.type)
                    isMatch = true;


                i++;
            } while (isMatch && i < 3);
        }
        else
        {
            if (_elem.posX > 1)
            {
                int i = 1;
                do
                {
                    if (allElements[_elem.posX - i, _elem.posY].type == _elem.type)
                        isMatch = true;
                    i++;
                } while (isMatch && i < 3);
            }

            if (_elem.posY > 1 && !isMatch)
            {
                int i = 1;
                do
                {
                    if (allElements[_elem.posX, _elem.posY - i].type == _elem.type)
                        isMatch = true;
                    i++;
                } while (isMatch && i < 3);
            }
        }

        return isMatch;
    }

    public override Element getElementFromPoint(int x, int y)
    {
        return allElements[x, y];
    }

    public override bool swipeElements(Element element1, Element element2)
    {
        Element tmp = element1.getElement();
        element1.setElement(element2);
        element2.setElement(tmp);

        // ЗАГЛУШКА
        return true;
    }
}
