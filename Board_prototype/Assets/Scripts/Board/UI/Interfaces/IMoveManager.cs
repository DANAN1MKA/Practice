using System.Collections.Generic;

public interface IMoveManager
{
    void dropElements(List<MovingElements> newList);

    void addElement(MovingElements newElement);

    void addElement(MovingElements newElement1, MovingElements newElement2);


    void removeElement(Element element);
}

