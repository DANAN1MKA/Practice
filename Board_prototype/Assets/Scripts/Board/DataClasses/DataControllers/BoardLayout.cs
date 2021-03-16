﻿using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class BoardLayout : MonoBehaviour
{
    [Inject] private SignalBus signalBus;

    [Inject] private IElementGenerator elementGenerator;

    [SerializeField] private BoardProperties config;

    private int width;
    private int heigth;
    private Vector2 boardPosition;
    private float scale;
    private float time;
    private float additionalTime;
    private bool isBlocked;
    private bool isItFirstMatch = false;

    private Element[,] board;
    private List<Element> foundMatches;

    public void Awake()
    {
        signalBus.Subscribe<SwipeElementSignal>(swipeElement);
        signalBus.Subscribe<TimerHandlerSignal>(timerHandler);
        signalBus.Subscribe<AnimationCompletedSignal>(animationCompleted);
    }

    void Start()
    {
        width = config.width;
        heigth = config.height;
        time = config.time;
        additionalTime = config.additionalTime;
        boardPosition = config.boardPositionFromResolution;
        scale = config.scale;

        board = elementGenerator.generateBoard(width, heigth);
        foundMatches = new List<Element>();
    }

    private bool isItMatch(Element element)
    {
        List<Element> matchElementsX = findHorizontalMatch(element);
        List<Element> matchElementsY = findVerticalMatch(element);

        if (matchElementsX.Count > 2 || matchElementsY.Count > 2)
        {
            if (matchElementsX.Count > 2) addMatches(matchElementsX);
            if (matchElementsY.Count > 2) addMatches(matchElementsY);

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

            if (neighbourRight.posX < width - 1) neighbourRight = board[neighbourRight.posX + 1, neighbourRight.posY];
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

            if (neighbourUp.posY < heigth - 1) neighbourUp = board[neighbourUp.posX, neighbourUp.posY + 1];
            else UpIsDone = true;

            if (!DownIsDone && neighbourDown.type == element.type) matchElementsY.Add(neighbourDown); else DownIsDone = true;
            if (!UpIsDone && neighbourUp.type == element.type) matchElementsY.Add(neighbourUp); else UpIsDone = true;
        }
        while (!UpIsDone || !DownIsDone);

        return matchElementsY;
    }

    private void addMatches(List<Element> _from)
    {
        foreach (Element elem in _from)
        {
            if (!foundMatches.Contains(elem))
            {
                foundMatches.Add(elem);

                elem.block();
            }
        }
    }

    public void swipeElement(SwipeElementSignal swipeElementSignal)
    {
        //TODO: разбить на методы
        int posX = swipeElementSignal.posX;
        int posY = swipeElementSignal.posY;
        int dirX = (int)swipeElementSignal.direction.x;
        int dirY = (int)swipeElementSignal.direction.y;

        if (!isBlocked && (dirX != 0 || dirY != 0))
        {
            if (!board[posX, posY].getState() && !board[posX + dirX, posY + dirY].getState())
            {
                swipe(board[posX, posY], 
                      board[posX + dirX, posY + dirY]);

                Vector2 targetPosition1 = board[posX, posY].position;
                Vector2 targetPosition2 = board[posX + dirX, posY + dirY].position;

                MovingElement elem1;
                MovingElement elem2;

                bool match1 = isItMatch(board[posX, posY]);
                bool match2 = isItMatch(board[posX + dirX, posY + dirY]);

                if (match1 || match2)
                {
                    elem1 = new MovingElement(board[posX, posY], targetPosition1, null);
                    elem2 = new MovingElement(board[posX + dirX, posY + dirY], targetPosition2, null);

                    timerSignalFire();
                }
                else
                {
                    MovingElement nextPositionElem1 = new MovingElement(board[posX, posY], targetPosition1, null);
                    elem1 = new MovingElement(board[posX, posY], targetPosition2, nextPositionElem1);

                    MovingElement nextPositionElem2 = new MovingElement(board[posX + dirX, posY + dirY], targetPosition2, null);
                    elem2 = new MovingElement(board[posX + dirX, posY + dirY], targetPosition1, nextPositionElem2);

                    swipe(board[posX, posY], 
                          board[posX + dirX, posY + dirY]);
                }


                signalBus.Fire(new MoveManagerAddSignal(elem1, elem2));

            }
        }
    }

    private void timerSignalFire()
    {
        if (!isItFirstMatch)
        {
            signalBus.Fire(new SetTimerSignal(time));
            isItFirstMatch = true;
        }
        else signalBus.Fire(new SetTimerSignal(additionalTime));
    }

    private void swipe(Element element1, Element element2)
    {
        Element tmp = element1.getElement();
        element1.setElement(element2);
        element2.setElement(tmp);
    }

    public void timerHandler()
    {
        isBlocked = true;
        isItFirstMatch = false;

        foundMatchesHandler();
    }

    public void animationCompleted()
    {
        if (isBlocked)
        {
            if (foundMatches.Count > 0)
            {
                foundMatchesHandler();
            }
            else isBlocked = false;
        }
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
        for (int j = 0; j < heigth; j++)
        {
            for (int i = shiftCounter; i < width; i += 3)
            {
                isItMatch(board[i, j]);
            }
            shiftCounter = shiftCounter < 2 ? shiftCounter + 1 : 0;
        }
    }

    private void foundMatchesHandler()
    {    
        foundMatches.Clear();

        handleBlockedElements();

        List<MovingElement> fallingElements = moveBlockedElementsUp();

        signalBus.Fire(new MoveManagerDropSignal(fallingElements));

        matchCascad();
    }

    private void handleBlockedElements()
    {
        for (int i = 0; i < heigth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (board[j, i].getState())
                {

                    elementGenerator.changeTypeCommon(board[j, i]);
                    int count = board[j, i].posY;

                    while (count < heigth - 1 && board[j, count].getState()) count++;

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

        int[] countForColumn = new int[width];

        for (int i = 0; i < heigth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector2 endPosition = board[j, i].position;

                fallingElements.Add(new MovingElement(board[j, i], endPosition, null));

                if (board[j, i].getState())
                {
                    board[j, i].piece.transform.position = new Vector2(boardPosition.x + board[j, i].posX * scale,
                                                                       boardPosition.y + (heigth + countForColumn[j]) * scale);

                    board[j, i].unblock();
                    countForColumn[j]++;
                }

                board[j, i].resetAnimanion();
            }
        }

        return fallingElements;
    }


}