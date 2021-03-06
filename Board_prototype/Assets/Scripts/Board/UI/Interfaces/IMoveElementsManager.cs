using System.Collections.Generic;

public interface IMoveElementsManager
{
    void dropElements(List<MovingElement> newList);

    void addElement(MovingElement newElement);

    void addElement(MovingElement newElement1, MovingElement newElement2);

    void removeElement(Element element);
}

