using UnityEngine;

public class ElementGenerator : MonoBehaviour, IElementGenerator
{
    [SerializeField] public BoardProperties config;

    private GameObject elementPrefab;
    private Material[] pool;
    private Vector2 boardPosition;

    private float defoultScreenWidthInUnits = 7.875f;

    private float elementScale;

    public void Awake()
    {
        elementPrefab = config.elementPrefab;
        pool = config.pool;
        boardPosition = config.boardPosition;

        float screenHeightInUnits = Camera.main.orthographicSize * 2;
        float screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height;

        elementScale = 2 - (defoultScreenWidthInUnits / screenWidthInUnits);
        boardPosition = boardPosition * elementScale;
        config.boardPositionFromResolution = boardPosition;
        config.scale = elementScale;
    }

    public void changeTypeCommon(Element element)
    {
        int newType = Random.Range(0, pool.Length);
        element.changeType(newType, pool[newType]);
    }

    public void changeTypeSpecial(Element element)
    {
        throw new System.NotImplementedException();
    }

    public Element createCommonPiece(int _posX, int _posY)
    {
        Vector2 position = new Vector2(boardPosition.x + _posX * elementScale,
                                       boardPosition.y + _posY * elementScale);

        GameObject newPiece = Instantiate(elementPrefab);
        Vector3 scale = new Vector3(elementScale, elementScale, elementScale);
        newPiece.transform.localScale = scale;

        Element newElement = new Element(newPiece);

        int type = Random.Range(0, pool.Length);
        newElement.changeType(type, pool[type]);

        newElement.piece.name = "( " + _posX + "," + _posY + " )";

        newElement.setPosition(_posX, _posY, position);

        return newElement;
    }

    public Element createSpecialPiece()
    {
        throw new System.NotImplementedException();
    }

    public Element[,] generateBoard(int _width, int _heigth)
    {
        Element[,] board = new Element[_width, _heigth];

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _heigth; j++)
            {
                Element newElement = createCommonPiece(i, j);

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
            isMatch = foundMatchBothDirections(allElements, _elem);
        }
        else
        {
            if (_elem.posX > 1)
            {
                isMatch = foundMatchLeft(allElements, _elem);
            }

            if (_elem.posY > 1 && !isMatch)
            {
                isMatch = foundMatchDown(allElements, _elem);
            }
        }

        return isMatch;
    }

    private bool foundMatchBothDirections(Element[,] allElements, Element _elem)
    {
        bool isMatch = false;
        int i = 1;
        do
        {
            if (allElements[_elem.posX - i, _elem.posY].type == _elem.type ||
                allElements[_elem.posX, _elem.posY - i].type == _elem.type)
                isMatch = true;
            else isMatch = false;

            i++;
        } while (isMatch && i < 3);

        return isMatch;
    }

    private bool foundMatchLeft(Element[,] allElements, Element _elem)
    {
        bool isMatch = false;

        int i = 1;
        do
        {
            if (allElements[_elem.posX - i, _elem.posY].type == _elem.type)
                isMatch = true;
            else isMatch = false;
            i++;
        } while (isMatch && i < 3);

        return isMatch;
    }

    private bool foundMatchDown(Element[,] allElements, Element _elem)
    {
        bool isMatch = false;

        int i = 1;
        do
        {
            if (allElements[_elem.posX, _elem.posY - i].type == _elem.type)
                isMatch = true;
            else isMatch = false;
            i++;
        } while (isMatch && i < 3);


        return isMatch;
    }
}
