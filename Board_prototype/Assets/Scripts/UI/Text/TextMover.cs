using UnityEngine;
using UnityEngine.UI;


public class TextMover : MonoBehaviour
{
    private bool isActive = false;
    Vector2 direction;

    public void setup(int scoreValue)
    {
        GetComponent<Text>().text = "+" + scoreValue;
        direction = Vector2.up * 150;
        isActive = true;
    }

    void Update()
    {
        if(isActive) transform.Translate(direction * Time.deltaTime);
    }
}
