using UnityEngine;
using UnityEngine.UI;


public class TextMover : MonoBehaviour
{
    private bool isActive = false;
    Vector2 direction;

    public void setup(System.UInt64 scoreValue)
    {
        GetComponent<Text>().text = "+" + scoreValue;
        float x = Random.Range(0.5f, -0.5f);
        direction = new Vector2(x, 1) * 150;
        isActive = true;
    }

    void Update()
    {
        if(isActive) transform.Translate(direction * Time.deltaTime);
    }
}
