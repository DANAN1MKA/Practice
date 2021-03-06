using UnityEngine;

public interface IElementGenerator
{
    Element createCommonPiece(int _posX, int _posY);

    Element createSpecialPiece();

    void changeTypeCommon(Element element);

    void changeTypeSpecial(Element element);

    Element[,] generateBoard(int _width, int _heigth, Transform _startPosition);
}
