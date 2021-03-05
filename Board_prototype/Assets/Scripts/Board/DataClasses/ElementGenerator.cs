using UnityEngine;

public class ElementGenerator : MonoBehaviour, IElementGenerator
{
    [SerializeField] public GameObject elementPrefab;
    [SerializeField] public Material[] pool;


    public void changeTypeCommon(Element element)
    {
        element.type = Random.Range(0, pool.Length);
        element.spriteRenderer.material = pool[element.type];
    }

    public void changeTypeSpecial(Element element)
    {
        throw new System.NotImplementedException();
    }

    public Element createCommonPiece(int _posX, int _posY)
    {
        GameObject newPiece = Instantiate(elementPrefab);
        Element newElement = new Element(newPiece);

        newElement.type = Random.Range(0, pool.Length);
        newElement.spriteRenderer.material = pool[newElement.type];

        newElement.piece.name = "( " + _posX + "," + _posY + " )";
        newElement.posX = _posX;
        newElement.posY = _posY;

        return newElement;
    }

    public Element createSpecialPiece()
    {
        throw new System.NotImplementedException();
    }

    public Element[,] generateBoard(int _width, int _heigth, Transform _startPosition)
    {
        Element[,] board = new Element[_width, _heigth];

        Vector2 curentPiecePosition = new Vector2(_startPosition.position.x, _startPosition.position.y);

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _heigth; j++)
            {
                Element newElement = createCommonPiece(i, j);
                newElement.piece.transform.parent = _startPosition;
                newElement.piece.transform.position = new Vector2(curentPiecePosition.x + i, curentPiecePosition.y + j);

                do
                {
                    changeTypeCommon(newElement);
                } while (isItInitMatch(board, newElement));

                board[i, j] = newElement;
            }
        }

        return board;
    }

    private bool isItInitMatch(Element[,] allElements, Element _elem)
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
}
