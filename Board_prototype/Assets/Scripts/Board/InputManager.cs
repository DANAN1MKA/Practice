using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputManager : MonoBehaviour
{
    public Vector2 SwipeStartPosition;
    public Vector2 SwipeDirection;

    [Inject] BoardFather board;

    private List<MovingElements> movingElemenets;

    Element currentElemenet;
    Vector2 currentElemenetBeganPositon;
    Vector2 currentElemenetEndPositon;
    public Vector2 currentDirection;

    int posX;
    int posY;
    public float elementBorders = 0.7f;
    bool isExistCurrElem = false;

    void Start()
    {
        movingElemenets = new List<MovingElements>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {


                case TouchPhase.Began:
                    SwipeStartPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    convertToElementPosition(SwipeStartPosition);

                    // если попали в доску
                    if (posX < board.width && posX >= 0 &&
                        posY < board.heigth && posX >= 0)
                    {
                        foreach (MovingElements current in movingElemenets)
                        {
                            //если мы уже двигали этот элемент получаем его
                            if (current.elem.posX == posX && current.elem.posY == posY)
                            {
                                currentElemenet = current.elem;
                                currentElemenetBeganPositon = current.endPosition;
                                currentElemenetEndPositon = currentElemenetBeganPositon;
                                SwipeStartPosition = currentElemenetBeganPositon;

                                movingElemenets.Remove(current);
                                isExistCurrElem = true;
                                break;
                            }
                        }

                        //если среди двигаемых не нашли, получаем элемент с доски
                        if (!isExistCurrElem)
                        {
                            currentElemenet = board.getElementFromPoint(posX, posY);
                            currentElemenetBeganPositon = new Vector2(board._thisTransform.position.x + posX, board._thisTransform.position.y + posY);
                            currentElemenetEndPositon = currentElemenetBeganPositon;
                            SwipeStartPosition = currentElemenetEndPositon;
                            isExistCurrElem = true;
                        }

                        currentElemenet.spriteRenderer.sortingOrder += 1;
                    }
                break;




                case TouchPhase.Moved:
                    if (isExistCurrElem)
                    {
                        SwipeDirection = SwipeDirection = (Vector2)Camera.main.ScreenToWorldPoint(touch.position) - SwipeStartPosition;

                        currentDirection = NormalizeDirection(SwipeDirection);

                        currentElemenetEndPositon = currentElemenetBeganPositon + (currentDirection / 2);
                        //currentElemenetEndPositon.x = currentElemenetBeganPositon.x + (currentDirection.x / 2);
                        //currentElemenetEndPositon.y = currentElemenetBeganPositon.y + (currentDirection.y / 2);
                    }
                break;




                case TouchPhase.Ended:
                    if (isExistCurrElem)
                    {
                        currentElemenet.spriteRenderer.sortingOrder -= 1;
                        if (currentDirection.x != 0 || currentDirection.y != 0)
                        {
                            //получаем второй элемент
                            Element elementTwo = null;
                            Vector2 elementTwoEndPosition = new Vector2();

                            bool findElementTwo = false;

                            //если мы уже двигали этот элемент получаем его
                            foreach (MovingElements current in movingElemenets)
                            {
                                if (current.elem.posX == currentElemenet.posX + currentDirection.x && current.elem.posY == currentElemenet.posY + currentDirection.y)
                                {
                                    elementTwo = current.elem;
                                    elementTwoEndPosition = current.endPosition;

                                    movingElemenets.Remove(current);
                                    findElementTwo = true;
                                    break;
                                }
                            }
                            if (!findElementTwo)
                            {
                                elementTwo = board.getElementFromPoint(currentElemenet.posX + (int)currentDirection.x, currentElemenet.posY + (int)currentDirection.y);
                                elementTwoEndPosition = new Vector2(board._thisTransform.position.x + elementTwo.posX, board._thisTransform.position.y + elementTwo.posY);
                            }

                            currentElemenetEndPositon = new Vector2(board._thisTransform.position.x + currentElemenet.posX, board._thisTransform.position.y + currentElemenet.posY);

                            //Если будет матч - будет свайп а пока ЗАГЛУШКА
                            if (board.swipeElements(currentElemenet, elementTwo))
                            {
                                movingElemenets.Add(new MovingElements(currentElemenet, currentElemenetEndPositon));
                                movingElemenets.Add(new MovingElements(elementTwo, elementTwoEndPosition));
                            }
                            else
                            {
                                movingElemenets.Add(new MovingElements(currentElemenet, currentElemenetBeganPositon));
                            }
                        }
                    }
                    currentElemenet = null;
                    currentDirection = new Vector2();
                    isExistCurrElem = false;
                    break;
            }
        }

        // Двигание элементов
        if (movingElemenets.Count > 0)
        {
            for (int i = 0; i < movingElemenets.Count; i++)
            {
                movingElemenets[i].elem.MoveElementTo(movingElemenets[i].endPosition);
                //if reached target position - remove element
                if (movingElemenets[i].elem.piece.transform.position.x == movingElemenets[i].endPosition.x &&
                    movingElemenets[i].elem.piece.transform.position.y == movingElemenets[i].endPosition.y)
                    movingElemenets.Remove(movingElemenets[i]);
            }
        }
        if (isExistCurrElem)
        {
            currentElemenet.MoveElementTo(currentElemenetEndPositon);
        }
    }

    private void convertToElementPosition(Vector2 position)
    {
        posX = Mathf.RoundToInt(board._thisTransform.position.x - position.x);
        posY = Mathf.RoundToInt(board._thisTransform.position.y - position.y);

        posX = posX < 0 ? posX * -1 : posX;
        posY = posY < 0 ? posY * -1 : posY;
    }
    public Vector2 NormalizeDirection(Vector2 direction)
    {
        Vector2 dir = new Vector2(0, 0);

        // if we left the borders of the element
        if (direction.x > elementBorders || direction.y > elementBorders ||
            direction.x < -elementBorders || direction.y < -elementBorders)
        {
            float angle = Vector2.Angle(direction, new Vector2(1, 0));

            if (angle <= 22) { dir.x = 1; dir.y = 0; } // right
            else
            if (angle > 22 && angle < 67) { dir.x = 1; dir.y = 1; } // top-right
            else
            if (angle >= 67 && angle < 112) { dir.x = 0; dir.y = 1; } // top
            else
            if (angle >= 112 && angle < 157) { dir.x = -1; dir.y = 1; } // top-left
            else
            if (angle >= 157) { dir.x = -1; dir.y = 0; } // left

            dir.y = direction.y < 0 ? dir.y * -1 : dir.y; // if we swiped down
        }

        // Normolize if we left the borders of the board
        dir.x = (posX + dir.x >= board.width || posX + dir.x < 0) ? 0 : dir.x;
        dir.y = (posY + dir.y >= board.heigth || posY + dir.y < 0) ? 0 : dir.y;

        return dir;
    }
}

public class MovingElements
{
    public Element elem;
    public Vector2 endPosition;

    public MovingElements(Element _elem, Vector2 _endPosition)
    {
        elem = _elem;
        endPosition = _endPosition;
    }
}

