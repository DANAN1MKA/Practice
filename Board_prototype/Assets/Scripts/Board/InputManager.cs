using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputManager : MonoBehaviour
{
    private Vector2 SwipeStartPosition;
    private Vector2 SwipeDirection;

    [Inject] private BoardFather board;
    [Inject] private IMoveManager moveManager;

    private Element currentElemenet;
    private Vector2 currentElemenetBeganPositon;
    private Vector2 currentElemenetEndPositon;
    private Vector2 currentDirection;

    private int posX;
    private int posY;
    private float elementBorders = 0.7f;
    private bool isExistCurrElem = false;

    void Update()
    {
                                    // TODO: надо переделать способ блокировки элементов    P.S. Не надо
        if (Input.touchCount > 0 && !board.isBoardBlocked)
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

                        currentElemenet = board.getElementFromPoint(posX, posY);
                        if (!currentElemenet.isBlocked)
                        {
                            currentElemenetBeganPositon = new Vector2(board._thisTransform.position.x + posX, board._thisTransform.position.y + posY);
                            currentElemenetEndPositon = currentElemenetBeganPositon;
                            SwipeStartPosition = currentElemenetEndPositon;
                            isExistCurrElem = true;
                           
                            moveManager.removeElement(currentElemenet);

                            currentElemenet.spriteRenderer.sortingOrder += 1;
                        }

                        else currentElemenet = null;
                    }
                break;




                case TouchPhase.Moved:
                    if (isExistCurrElem)
                    {
                        SwipeDirection = SwipeDirection = (Vector2)Camera.main.ScreenToWorldPoint(touch.position) - SwipeStartPosition;
                        currentDirection = NormalizeDirection(SwipeDirection);
                        currentElemenetEndPositon = currentElemenetBeganPositon + (currentDirection / 2);
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

                            elementTwo = board.getElementFromPoint(currentElemenet.posX + (int)currentDirection.x, currentElemenet.posY + (int)currentDirection.y);
                            moveManager.removeElement(elementTwo);

                            elementTwoEndPosition = new Vector2(board._thisTransform.position.x + elementTwo.posX, board._thisTransform.position.y + elementTwo.posY);
                            currentElemenetEndPositon = new Vector2(board._thisTransform.position.x + currentElemenet.posX, board._thisTransform.position.y + currentElemenet.posY);

                            if (board.swipeElements(currentElemenet, elementTwo))
                            {
                                moveManager.addElement(new MovingElements(currentElemenet, currentElemenetEndPositon), 
                                                       new MovingElements(elementTwo, elementTwoEndPosition));
                            }
                            else
                            {
                                moveManager.addElement(new MovingElements(currentElemenet, currentElemenetBeganPositon));
                            }
                        }
                        else moveManager.addElement(new MovingElements(currentElemenet, currentElemenetBeganPositon));
                    }
                    currentElemenet = null;
                    currentDirection = new Vector2();
                    isExistCurrElem = false;
                    break;
            }
        }

        //Двигаем текущий элемент
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

        if (direction.x > elementBorders || direction.y > elementBorders ||
            direction.x < -elementBorders || direction.y < -elementBorders)
        {
            dir = direction / direction.magnitude;

            dir.x = dir.x > 0.5f ? 1 : 
                    dir.x < -0.5f ? -1 : 0;

            dir.y = dir.y > 0.5f ? 1 :
                    dir.y < -0.5f ? -1 : 0;

            // Normolize if we left the borders of the board
            dir.x = (posX + dir.x >= board.width || posX + dir.x < 0) ? 0 : dir.x;
            dir.y = (posY + dir.y >= board.heigth || posY + dir.y < 0) ? 0 : dir.y;
        }
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

