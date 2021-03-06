using System.Collections.Generic;

public interface IMoveElementsManager
{
    void dropElements(List<MovingElement> newList);

    void addElement(MovingElement newElement1, MovingElement newElement2);
}

