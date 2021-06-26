using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardReplayServerLayout : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private BoardProperties config;

    [Inject] private IElementGenerator elementGenerator;

    private Element[,] board;
    private SetStepJSON history = null;
    private bool isActive;

    private LiensList liensList;


    public void Awake()
    {
        //signalBus.Subscribe<ServerReplaySignal>
        signalBus.Subscribe<ServerReplaySignal>(newReplay);
        //signalBus.Subscribe<KillingCompletedSignal>(startReplay);
        signalBus.Subscribe<AnimationCompletedSignal>(nextStep);
    }

    public void Start()
    {
        board = elementGenerator.generateReplayBoard(this.transform, config.width, config.height);
        hide();
    }


    private void newReplay(ServerReplaySignal signal)
    {
        history = signal.json;

        int counter = 0;

        for (int i = 0; i < config.width; i++)
            for (int j = 0; j < config.height; j++)
            {
                elementGenerator.changeTypeCommon(board[i, j], history.board[counter].type);
                counter++;
            }
        startReplay();
    }

    private int swipeCounter;

    private void startReplay()
    {
        if (history != null)
        {
            swipeCounter = 0;
            newGemsTypeCounter = 0;
            isActive = true;
            show();

            swipeElement(history.swipeHistory[swipeCounter]);
            swipeCounter++;
        }
    }

    private void nextStep()
    {
        if (isActive)
        {
            if (swipeCounter < history.swipeHistory.Length)
            {
                if (liensList != null) signalBus.Fire(new RenderLineSignal(liensList));
                swipeElement(history.swipeHistory[swipeCounter]);
                swipeCounter++;
            }
            else
            if (newGemsTypeCounter < history.newGemsType.Length)
            {
                foundMatchesHandler();
                //сбрасываем линии
                signalBus.Fire<TimerHandlerSignal>();
            }
            else
            {
                isActive = false;
                history = null;

                hide();
                signalBus.Fire<ReplayCompliteSignal>();
            }
        }
    }

    public void swipeElement(SwipeHistory swipeData)
    {
        liensList = null;
        //TODO: разбить на методы
        int posX = swipeData.posX;
        int posY = swipeData.posY;
        int dirX = (int)swipeData.directionX;
        int dirY = (int)swipeData.directionY;


        swipe(board[posX, posY],
              board[posX + dirX, posY + dirY]);

        Vector2 targetPositionTouched = board[posX, posY].position;
        Vector2 targetPositionNeighbour = board[posX + dirX, posY + dirY].position;

        MovingElement elemTouched;
        MovingElement elemNeighbour;

        bool match1 = isItMatch(board[posX, posY]);
        bool match2 = isItMatch(board[posX + dirX, posY + dirY]);

        if (match1 || match2)
        {
            elemTouched = new MovingElement(board[posX, posY], targetPositionTouched, null);
            elemNeighbour = new MovingElement(board[posX + dirX, posY + dirY], targetPositionNeighbour, null);

        }
        else
        {
            MovingElement nextPositionElemTouched = new MovingElement(board[posX, posY], targetPositionTouched, null);
            elemTouched = new MovingElement(board[posX, posY], targetPositionNeighbour, nextPositionElemTouched);

            MovingElement nextPositionElemNeighbour = new MovingElement(board[posX + dirX, posY + dirY], targetPositionNeighbour, null);
            elemNeighbour = new MovingElement(board[posX + dirX, posY + dirY], targetPositionTouched, nextPositionElemNeighbour);

            swipe(board[posX, posY],
                  board[posX + dirX, posY + dirY]);
        }
        elemTouched.elem.move(elemTouched.endPosition);
        elemNeighbour.elem.move(elemNeighbour.endPosition);

        signalBus.Fire(new MoveManagerSwipeSignal(elemTouched, elemNeighbour));

    }

    private bool isItMatch(Element element)
    {
        List<Element> matchElementsX = findHorizontalMatch(element);
        List<Element> matchElementsY = findVerticalMatch(element);

        if (matchElementsX.Count > 2 || matchElementsY.Count > 2)
        {
            generateLinesList(element, matchElementsX, matchElementsY);

            if (matchElementsX.Count > 2) foreach (Element curr in matchElementsX) curr.block();
            if (matchElementsY.Count > 2) foreach (Element curr in matchElementsY) curr.block();

            return true;
        }
        else return false;
    }

    private List<Element> findHorizontalMatch(Element element)
    {
        List<Element> matchElementsX = new List<Element>();
        matchElementsX.Add(element);

        Element neighbourLeft = element; bool leftIsDone = false;
        Element neighbourRight = element; bool rightIsDone = false;

        do
        {
            if (neighbourLeft.posX > 0) neighbourLeft = board[neighbourLeft.posX - 1, neighbourLeft.posY];
            else leftIsDone = true;

            if (neighbourRight.posX < config.width - 1) neighbourRight = board[neighbourRight.posX + 1, neighbourRight.posY];
            else rightIsDone = true;

            if (!leftIsDone && neighbourLeft.type == element.type) matchElementsX.Add(neighbourLeft); else leftIsDone = true;
            if (!rightIsDone && neighbourRight.type == element.type) matchElementsX.Add(neighbourRight); else rightIsDone = true;
        }
        while (!leftIsDone || !rightIsDone);

        return matchElementsX;
    }

    private List<Element> findVerticalMatch(Element element)
    {
        List<Element> matchElementsY = new List<Element>();
        matchElementsY.Add(element);

        Element neighbourDown = element; bool DownIsDone = false;
        Element neighbourUp = element; bool UpIsDone = false;

        do
        {
            if (neighbourDown.posY > 0) neighbourDown = board[neighbourDown.posX, neighbourDown.posY - 1];
            else DownIsDone = true;

            if (neighbourUp.posY < config.height - 1) neighbourUp = board[neighbourUp.posX, neighbourUp.posY + 1];
            else UpIsDone = true;

            if (!DownIsDone && neighbourDown.type == element.type) matchElementsY.Add(neighbourDown); else DownIsDone = true;
            if (!UpIsDone && neighbourUp.type == element.type) matchElementsY.Add(neighbourUp); else UpIsDone = true;
        }
        while (!UpIsDone || !DownIsDone);

        return matchElementsY;
    }

    private void swipe(Element element1, Element element2)
    {
        Element tmp = element1.getElement();
        element1.setElement(element2);
        element2.setElement(tmp);
    }


    private int newGemsTypeCounter;

    private void foundMatchesHandler()
    {
        handleBlockedElements();

        List<MovingElement> fallingElements = moveBlockedElementsUp();

        foreach (MovingElement curr in fallingElements)
        {
            curr.elem.move(curr.endPosition);
        }

        signalBus.Fire(new MoveManagerDropSignal(fallingElements));

        matchCascad();
    }

    private void matchCascad()
    {

        /*
         на поле проверяем только элементы помеченые единицой
         * 1 * * 1 * *
         1 * * 1 * * 1
         * * 1 * * 1 *
         * 1 * * 1 * *
         1 * * 1 * * 1
         * * 1 * * 1 *
         * 1 * * 1 * *
         1 * * 1 * * 1
         */
        int shiftCounter = 0;
        for (int j = 0; j < config.height; j++)
        {
            for (int i = shiftCounter; i < config.width; i += 3)
            {
                isItMatch(board[i, j]);
            }
            shiftCounter = shiftCounter < 2 ? shiftCounter + 1 : 0;
        }
    }


    private void handleBlockedElements()
    {
        for (int i = 0; i < config.height; i++)
        {
            for (int j = 0; j < config.width; j++)
            {
                if (board[j, i].getState())
                {

                    int count = board[j, i].posY;

                    while (count < config.height - 1 && board[j, count].getState()) count++;

                    if (!board[j, count].getState())
                    {
                        swipe(board[j, i], board[j, count]);

                        board[j, i].unblock();
                        board[j, count].block();

                    }
                }
            }
        }
    }

    private List<MovingElement> moveBlockedElementsUp()
    {

        List<MovingElement> fallingElements = new List<MovingElement>();

        int[] countForColumn = new int[config.width];

        for (int i = 0; i < config.height; i++)
        {
            for (int j = 0; j < config.width; j++)
            {
                Vector2 endPosition = board[j, i].position;

                fallingElements.Add(new MovingElement(board[j, i], endPosition, null));

                if (board[j, i].getState())
                {

                    elementGenerator.changeTypeCommon(board[j, i], history.newGemsType[newGemsTypeCounter]);

                    newGemsTypeCounter++;


                    board[j, i].piece.transform.position = new Vector2(config.boardPositionFromResolution.x + board[j, i].posX * config.scale,
                                                                       config.boardPositionFromResolution.y + (config.height + countForColumn[j]) * config.scale);

                    board[j, i].unblock();
                    countForColumn[j]++;
                }

                board[j, i].resetAnimanion();
            }
        }

        return fallingElements;
    }


    private void generateLinesList(Element element, List<Element> _horizontalMatch, List<Element> _verticalMatch)
    {
        if (_horizontalMatch.Count > 2)
        {
            Vector3[] points = findPositionsForLine(element, _horizontalMatch);
            LiensList nextLine = liensList;
            liensList = new LiensList(points, element.type, nextLine);
        }
        if (_verticalMatch.Count > 2)
        {
            Vector3[] points = findPositionsForLine(element, _verticalMatch);
            LiensList nextLine = liensList;
            liensList = new LiensList(points, element.type, nextLine);
        }
    }

    private Vector3[] findPositionsForLine(Element element, List<Element> match)
    {
        Vector2 startPoint = element.position;
        Vector2 endPoint = element.position;

        bool isHorizontal = match[0].posY == match[1].posY;

        foreach (Element curr in match)
        {
            if (isHorizontal)
            {
                if (startPoint.x < curr.position.x) startPoint = curr.position;
                if (startPoint.x > curr.position.x) endPoint = curr.position;
            }
            else
            {
                if (startPoint.y < curr.position.y) startPoint = curr.position;
                if (startPoint.y > curr.position.y) endPoint = curr.position;
            }
        }

        return new Vector3[] { startPoint, endPoint };
    }

    private void show()
    {
        for (int i = 0; i < config.width; i++)
            for (int j = 0; j < config.height; j++)
            {
                board[i, j].show();
            }
    }
    private void hide()
    {
        for (int i = 0; i < config.width; i++)
            for (int j = 0; j < config.height; j++)
            {
                board[i, j].hide();
            }
    }

}
