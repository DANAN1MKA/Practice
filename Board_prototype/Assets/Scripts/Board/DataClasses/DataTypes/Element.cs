using UnityEngine;

public class Element
{
    private float speed = 6f;


    //TODO: переписать так чтобы можно было читать и нельзя изменять
    public int type { get; private set; }

    public int posX { get; private set; }
    public int posY { get; private set; }

    public Vector2 position { get; private set; }

    private bool isBlocked;

    public GameObject piece { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public Animator animator { get; private set; }

    public Element(GameObject _piece)
    {
        setPiece(_piece);
    }

    private Element(GameObject _element, SpriteRenderer _spriteRenderer, Animator _animator)
    {
        piece = _element;
        spriteRenderer = _spriteRenderer;
        animator = _animator;
    }

    public void changeType(int _type, Material _material)
    {
        type = _type;
        spriteRenderer.material = _material;
    }

    public void setPosition(int _posX, int _posY, Vector2 _position)
    {
        posX = _posX;
        posY = _posY;
        position = _position;

        piece.transform.position = _position;

    }

    public void setPiece(GameObject _piece)
    {
        piece = _piece;
        spriteRenderer = piece.GetComponent<SpriteRenderer>();
        animator = piece.GetComponent<Animator>();
    }

    public void setElement(Element elem)
    {
        piece = elem.piece;
        type = elem.type;
        spriteRenderer = elem.spriteRenderer;
        animator = elem.animator;
    }

    public void moveHard(Vector2 move)
    {
        piece.transform.position = Vector2.MoveTowards(piece.transform.position, move, Time.deltaTime * speed);
    }

    public Element getElement()
    {
        Element clone = new Element(this.piece, this.spriteRenderer, this.animator);
        clone.type = this.type;

        return clone;
    }

    public void block()
    {
        animator.StartPlayback();
        animator.Play(0);
        isBlocked = true;
    }

    public void unblock()
    {
        animator.StopPlayback();

        isBlocked = false;
    }

    public bool getState() { return isBlocked; }

    public void resetAnimanion()
    {
        animator.StopPlayback();
        animator.Play(0);
    }

}
