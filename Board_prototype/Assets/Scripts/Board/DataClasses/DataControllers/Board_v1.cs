using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Board_v1 : BoardFather
{
    [Inject] private SignalBus signalBus;

    //TODO: убрать когда доделаю ивенты
    [Inject] private IMoveElementsManager moveController;
    [Inject] private IElementGenerator elementGenerator;

    private int width;
    private int heigth;
    private float time;
    private float additionalTime;
    private bool isBlocked;
    private bool isItFirstMatch = false;

    private Element[,] board;
    private List<Element> foundMatches;

    void Start()
    {
        signalBus.Subscribe<GrabElemetnSignal>(grabElement);
        signalBus.Subscribe<SwipeElementSignal>(swipeElement);
        signalBus.Subscribe<TimerHandlerSignal>(timerHandler);
        signalBus.Subscribe<AnimationCompletedSignal>(animationCompleted);


        width = config.width;
        heigth = config.height;
        time = config.time;
        additionalTime = config.additionalTime;

        board = elementGenerator.generateBoard(width, heigth, _thisTransform);
        foundMatches = new List<Element>();
    }

    private bool isItMatch(Element element)
    {
        List<Element> matchElementsX = new List<Element>();
        List<Element> matchElementsY = new List<Element>();
        matchElementsX.Add(element);
        matchElementsY.Add(element);

        //смотрим матчи по горизонтали
        Element neighbourLeft = element; bool leftIsDone = false;
        Element neighbourRight = element; bool rightIsDone = false;

        do
        {
            if (neighbourLeft.posX > 0) neighbourLeft = board[neighbourLeft.posX - 1, neighbourLeft.posY];
            else leftIsDone = true;

            if (neighbourRight.posX < width - 1) neighbourRight = board[neighbourRight.posX + 1, neighbourRight.posY];
            else rightIsDone = true;
            //else break;

            if (!leftIsDone && neighbourLeft.type == element.type) matchElementsX.Add(neighbourLeft); else leftIsDone = true;
            if (!rightIsDone && neighbourRight.type == element.type) matchElementsX.Add(neighbourRight); else rightIsDone = true;
        }
        while (!leftIsDone || !rightIsDone);

        //смотрим матчи по вертикали
        Element neighbourDown = element; bool DownIsDone = false;
        Element neighbourUp = element; bool UpIsDone = false;

        do
        {
            if (neighbourDown.posY > 0) neighbourDown = board[neighbourDown.posX, neighbourDown.posY - 1];
            else DownIsDone = true;

            if (neighbourUp.posY < heigth - 1) neighbourUp = board[neighbourUp.posX, neighbourUp.posY + 1];
            else UpIsDone = true;
            //else break;

            if (!DownIsDone && neighbourDown.type == element.type) matchElementsY.Add(neighbourDown); else DownIsDone = true;
            if (!UpIsDone && neighbourUp.type == element.type) matchElementsY.Add(neighbourUp); else UpIsDone = true;
        }
        while (!UpIsDone || !DownIsDone);

        //Если найдены матчи возвращаем правду иначе ложь
        if (matchElementsX.Count > 2 || matchElementsY.Count > 2)
        {
            //Запоминем найденные матчи
            if (matchElementsX.Count > 2)
            {
                addMatches(matchElementsX);
            }
            if (matchElementsY.Count > 2)
            {
                addMatches(matchElementsY);
            }

            return true;
        }
        else return false;
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

    public override void grabElement(GrabElemetnSignal _grabElemetnSignal)
    {
        if(!isBlocked)
            if (!board[_grabElemetnSignal.posX, _grabElemetnSignal.posY].getState())
            {
                board[_grabElemetnSignal.posX, _grabElemetnSignal.posY].block();
            }
    }

    public override void swipeElement(SwipeElementSignal swipeElementSignal)
    {
        int posX = swipeElementSignal.posX;
        int posY = swipeElementSignal.posY;
        int dirX = (int)swipeElementSignal.direction.x;
        int dirY = (int)swipeElementSignal.direction.y;

        if (!isBlocked && (dirX != 0 || dirY != 0))
        {
            if (!board[posX + dirX, 
                       posY + dirY].getState())
            {
                swipe(board[posX, posY], 
                      board[posX + dirX, posY + dirY]);

                bool match1 = isItMatch(board[posX, posY]);
                bool match2 = isItMatch(board[posX + dirX, posY + dirY]);

                if (match1 || match2)
                {
                    if (match1) board[posX, posY].block(); 
                    else board[posX, posY].unblock();

                    if (match2) board[posX + dirX, posY + dirY].block();
                    else board[posX + dirX, posY + dirY].unblock();

                    List<Vector2> position1 = new List<Vector2>();
                    List<Vector2> position2 = new List<Vector2>();

                    Vector2 targetPosition1 = calculateTargetPosition(posX, posY);
                    Vector2 targetPosition2 = calculateTargetPosition(posX + dirX, posY + dirY);

                    position1.Add(targetPosition1);
                    position2.Add(targetPosition2);

                    moveController.addElement(new MovingElement(board[posX, posY], position1),
                                              new MovingElement(board[posX + dirX, posY + dirY], position2));

                    if (!isItFirstMatch)
                    {
                        signalBus.Fire(new SetTimerSignal(time));
                        isItFirstMatch = true;
                    }
                    else signalBus.Fire(new SetTimerSignal(additionalTime));

                }
                else
                {

                    List<Vector2> position1 = new List<Vector2>();
                    List<Vector2> position2 = new List<Vector2>();

                    Vector2 targetPosition1 = calculateTargetPosition(posX, posY);
                    Vector2 targetPosition2 = calculateTargetPosition(posX + dirX, posY + dirY);

                    position1.Add(targetPosition2);
                    position1.Add(targetPosition1);

                    position2.Add(targetPosition1);
                    position2.Add(targetPosition2);

                    moveController.addElement(new MovingElement(board[posX, posY], position1),
                                              new MovingElement(board[posX + dirX, posY + dirY], position2));

                    board[posX, posY].unblock();
                    board[posX + dirX, posY + dirY].unblock();

                    swipe(board[posX, posY], 
                          board[posX + dirX, posY + dirY]);
                }
            }
            else board[posX, posY].unblock();
        }
        else board[posX, posY].unblock();
    }


    public Vector2 calculateTargetPosition(int _posX, int _posY)
    {
        Vector2 position = new Vector2(_thisTransform.position.x + _posX,
                                       _thisTransform.position.y + _posY);

        return position;
    }


    private void swipe(Element element1, Element element2)
    {
        Element tmp = element1.getElement();
        element1.setElement(element2);
        element2.setElement(tmp);
    }

    public override void timerHandler()
    {
        isBlocked = true;
        isItFirstMatch = false;

        foundMatchesHandler();
    }

    public override void animationCompleted()
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

        List<MovingElement> fallingElements = new List<MovingElement>();

        int[] countForColumn = new int[width];

        for (int i = 0; i < heigth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //если элемент в матче 
                if (board[j, i].getState())
                {

                    elementGenerator.changeTypeCommon(board[j, i]);
                    countForColumn[j]++;


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

        countForColumn = new int[width];

        for (int i = 0; i < heigth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                List<Vector2> endPosition = new List<Vector2>();
                endPosition.Add(new Vector2(_thisTransform.position.x + board[j, i].posX,
                                            _thisTransform.position.y + board[j, i].posY));

                fallingElements.Add(new MovingElement(board[j, i], endPosition));

                if (board[j, i].getState())
                {
                    board[j, i].piece.transform.position = new Vector2(_thisTransform.position.x + board[j, i].posX, _thisTransform.position.y + heigth + countForColumn[j]);

                    board[j, i].unblock();
                    countForColumn[j]++;
                }

                board[j, i].resetAnimanion();
            }
        }

        moveController.dropElements(fallingElements);

        matchCascad();
    }

}






//TODO: на случай если новая система не будет работать правильно
//Vector2 element1 = new Vector2(_thisTransform.position.x + board[swipeElementSignal.posX,
//                                                                 swipeElementSignal.posY].posX,
//                               _thisTransform.position.y + board[swipeElementSignal.posX,
//                                                                 swipeElementSignal.posY].posY);
//List<Vector2> elementEndPos1 = new List<Vector2>();
//elementEndPos1.Add(element1);


//Vector2 element2 = new Vector2(_thisTransform.position.x + board[swipeElementSignal.posX + (int)swipeElementSignal.direction.x,
//                                                                 swipeElementSignal.posY + (int)swipeElementSignal.direction.y].posX,
//                               _thisTransform.position.y + board[swipeElementSignal.posX + (int)swipeElementSignal.direction.x,
//                                                                 swipeElementSignal.posY + (int)swipeElementSignal.direction.y].posY);
//List<Vector2> elementEndPos2 = new List<Vector2>();
//elementEndPos2.Add(element2);

