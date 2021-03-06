using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputController : MonoBehaviour
{
    private Vector2 SwipeStartPosition;
    private Vector2 SwipeDirection;

    [Inject] private BoardFather board;
    private Vector2 currentDirection;

    private int posX;
    private int posY;
    private float elementBorders = 0.7f;
    private bool isExistCurrElem = false;

    void Update()
    {
        if (Input.touchCount > 0 && !board.getState())
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
                        isExistCurrElem = board.grabElement(posX, posY);
                    }
                    break;


                case TouchPhase.Moved:
                    if (isExistCurrElem)
                    {
                        SwipeDirection = SwipeDirection = (Vector2)Camera.main.ScreenToWorldPoint(touch.position) - SwipeStartPosition;
                        currentDirection = NormalizeDirection(SwipeDirection);
                    }
                    break;


                case TouchPhase.Ended:
                    if (isExistCurrElem)
                    {
                        board.swipeElement(posX, posY, currentDirection);
                    }
                    currentDirection = new Vector2();
                    isExistCurrElem = false;
                    break;

            }
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
