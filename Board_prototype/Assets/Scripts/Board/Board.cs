﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Board : BoardFather
{
    [Inject] private Itimer timer;
    [SerializeField] float time;
    [SerializeField] float additionalTime;

    private bool isItFirstMatch = false; //проверка на первый матч

    [Inject] private IMoveManager moveManager;

    [Inject] private IPieceGenerator pieceGenerator;

    private Element[,] allElements;



    private List<Element> foundMatches;



    void Start()
    {
        initBoard();

        foundMatches = new List<Element>();

        //сообщаем прогрес бару сколько секунд мы отсчитываем 
        timer.setConfigProgresBar(time);

        //timer.setTimer(time);
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

    private bool isItMatch(Element element)
    {
        List<Element> matchElementsX = new List<Element>();
        List<Element> matchElementsY = new List<Element>();
        matchElementsX.Add(element);
        matchElementsY.Add(element);

        //смотрим матчи по горизонтали
        Element neighbourLeft = element;  bool leftIsDone = false;
        Element neighbourRight = element; bool rightIsDone = false;

        do
        {
            if (neighbourLeft.posX > 0) neighbourLeft = allElements[neighbourLeft.posX - 1, neighbourLeft.posY];
            else leftIsDone = true;

            if (neighbourRight.posX < width - 1) neighbourRight = allElements[neighbourRight.posX + 1, neighbourRight.posY];
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
            if (neighbourDown.posY > 0) neighbourDown = allElements[neighbourDown.posX, neighbourDown.posY - 1];
            else DownIsDone = true;

            if (neighbourUp.posY < heigth - 1) neighbourUp = allElements[neighbourUp.posX, neighbourUp.posY + 1];
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
                addNewElementsTo(foundMatches, matchElementsX);

                // TODO: Костыль
                if (!isItFirstMatch)
                {

                    timer.setTimer(time);
                    isItFirstMatch = true;
                }
                else timer.setTimer(additionalTime);

            }
            if (matchElementsY.Count > 2)
            {
                addNewElementsTo(foundMatches, matchElementsY);

                // TODO: Костыль
                if (!isItFirstMatch)
                {

                    timer.setTimer(time);
                    isItFirstMatch = true;
                }
                else timer.setTimer(additionalTime);
            }

            return true;
        }
        else return false;
    }

    private void addNewElementsTo(List<Element> _to, List<Element> _from)
    {
        foreach(Element elem in _from)
        {
            if (!_to.Contains(elem))
            {

                _to.Add(elem);

                //блокируем добавляемые элементы
                elem.isBlocked = true;

                // TODO: почему не работает?
                elem.spriteRenderer.color = new Color(1, 1, 1,255);
                //tmp
            }
        }
    }

    public override Element getElementFromPoint(int x, int y)
    {
        return allElements[x, y];
    }

    public override bool swipeElements(Element element1, Element element2)
    {
        if (!element1.isBlocked && !element2.isBlocked)
        {
            swipe(element1, element2);

            bool match1 = isItMatch(element1);
            bool match2 = isItMatch(element2);

            if (match1 || match2)
            {
                // TODO: Мб перенести в метод поиска матчей?
                //if (!crutch) 
                //{ 
                    
                //    timer.setTimer(time);
                //    crutch = true;
                //}
                //else timer.setTimer(additionalTime);


                return true;
            }
            else
            {
                swipe(element1, element2);
                return false;
            }
        }
        return false;
    }

    private void swipe(Element element1, Element element2)
    {
        Element tmp = element1.getElement();
        element1.setElement(element2);
        element2.setElement(tmp);
    }



    // TODO: нужно переписать нормально
    public override void timerHandler()
    {
        //isBoardBlocked = true;
        isItFirstMatch = false;

        // TODO: список элементов что находятся в матче спорное решение?
        foundMatches.Clear();


        //список элементов что должны упасть вниз
        List<MovingElements> fallingElements = new List<MovingElements>();

        //количество позиций на которое должен упасть элемент СКОРЕЕ всего спорное решение
        int[] countForColumn = new int[width];


        for (int i = 0; i < heigth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //если элемент в матче 
                if(allElements[j, i].isBlocked)
                {

                    pieceGenerator.changeType(allElements[j, i]);
                    countForColumn[j]++;


                    int count = allElements[j,i].posY;

                    while (count < heigth - 1 && allElements[j, count].isBlocked) count++;

                    if (!allElements[j, count].isBlocked)
                    {
                        swipe(allElements[j, i], allElements[j, count]);

                        allElements[j, i].isBlocked = false;
                        //allElements[j, i].piece.transform.position = new Vector2(allElements[j, i].piece.transform.position.x, _thisTransform.position.y + allElements[j, i].posY);

                        allElements[j, count].isBlocked = true;                                                                                                                           //TODO: сдеть красивше
                        //allElements[j, count].piece.transform.position = new Vector2(allElements[j, i].piece.transform.position.x, _thisTransform.position.y + heigth + countForColumn[j] - 1);

                    }
                    //else
                    //allElements[j, i].piece.transform.position = new Vector2(allElements[j, i].piece.transform.position.x, _thisTransform.position.y + heigth + countForColumn[j]);

                }
            }
        }

        countForColumn = new int[width];

        for (int i = 0; i < heigth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                fallingElements.Add(new MovingElements(
                    allElements[j, i],
                    new Vector2(_thisTransform.position.x + allElements[j, i].posX, 
                                _thisTransform.position.y + allElements[j, i].posY)));

                if(allElements[j, i].isBlocked)
                {
                    allElements[j, i].piece.transform.position = new Vector2(_thisTransform.position.x + allElements[j, i].posX, _thisTransform.position.y + heigth + countForColumn[j]);
                    allElements[j, i].isBlocked = false;
                    countForColumn[j]++;

                }
            }
        }


        moveManager.dropElements(fallingElements);
                //ustroyDestroy();
                //initBoard();
                //isBoardBlocked = false;
                // TODO: Add logic
    }

    private void ustroyDestroy()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < heigth; j++)
            {
                Destroy(allElements[i, j].piece);
                allElements[i, j] = null;
            }
    }

}