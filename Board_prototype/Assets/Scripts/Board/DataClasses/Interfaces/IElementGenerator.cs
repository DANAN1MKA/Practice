using UnityEngine;

public interface IElementGenerator
{
    Element createCommonPiece(int _posX, int _posY);
    Element createReplayPiece(Transform repalayTransform, int _posX, int _posY);


    Element createSpecialPiece();

    void changeTypeCommon(Element element);
    void changeTypeCommon(Element element, int type);


    void changeTypeSpecial(Element element);

    Element[,] generateBoard(int _width, int _heigth);

    Element[,] generateReplayBoard(Transform repalayTransform, int _width, int _heigth);
}
