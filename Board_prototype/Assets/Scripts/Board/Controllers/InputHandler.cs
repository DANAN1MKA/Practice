using UnityEngine;
using Zenject;

public class InputHandler : MonoBehaviour
{
    private Vector2 SwipeStartPosition;
    private Vector2 SwipeDirection;

    [Inject] private SignalBus signalBus;

    public BoardConfig config;
    private Vector2 boardPosition;
    private int width;
    private int heigth;

    private int posX;
    private int posY;
    private Vector2 currentDirection;
    private float elementBorders = 0.7f;
    private bool isExistCurrElem = false;

    private void Start()
    {
        width = config.width;
        heigth = config.height;
        boardPosition = config.boardPositionFromResolution;
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
                    if (posX < width && posX >= 0 &&
                        posY < heigth && posX >= 0)
                    { isExistCurrElem = true; }
                    break;


                case TouchPhase.Moved:
                    if (isExistCurrElem)
                    {
                        SwipeDirection = (Vector2)Camera.main.ScreenToWorldPoint(touch.position) - SwipeStartPosition;
                        currentDirection = normalizeDirection(SwipeDirection);

                        if(currentDirection.x != 0 || currentDirection.y  != 0)
                        {
                            signalBus.Fire(new SwipeElementSignal(posX, posY, currentDirection));
                            currentDirection = new Vector2();
                            isExistCurrElem = false;

                        }

                    }
                    break;


                case TouchPhase.Ended:
                    currentDirection = new Vector2();
                    isExistCurrElem = false;
                    break;
            }
        }

    }

    private void convertToElementPosition(Vector2 position)
    {
        posX = Mathf.RoundToInt((position.x - boardPosition.x) / config.scale);
        posY = Mathf.RoundToInt((position.y - boardPosition.y) / config.scale);

        posX = posX < 0 ? posX * -1 : posX;
        posY = posY < 0 ? posY * -1 : posY;
    }

    public Vector2 normalizeDirection(Vector2 direction)
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
            dir.x = (posX + dir.x >= width || posX + dir.x < 0) ? 0 : dir.x;
            dir.y = (posY + dir.y >= heigth || posY + dir.y < 0) ? 0 : dir.y;
        }
        return dir;
    }

}
