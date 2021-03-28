using UnityEngine;

public class PersonalGemMover : MonoBehaviour
{
    private bool isActive = false;
    private Vector2 targetPosition;
    [SerializeField] private float speed;

    public delegate void HostElement();
    private HostElement hostMethod;
    public void setNewHost(HostElement callback)
    {
        hostMethod = callback;
    }

    void Update()
    {
        if (isActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

            if (isOnPosition())
            {
                isActive = false;
                hostMethod();
            }
        }
    }

    public void setNewPosition(Vector2 _newPosition)
    {
        targetPosition = _newPosition;
        isActive = true;
    }

    private bool isOnPosition()
    {
        return (transform.position.x == targetPosition.x &&
                transform.position.y == targetPosition.y);
    }
}
