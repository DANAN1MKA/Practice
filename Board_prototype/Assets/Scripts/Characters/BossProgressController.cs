using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BossProgressController : MonoBehaviour
{
    private Slider slider;

    [Inject] ICheracterController cheracterController;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        cheracterController.setUI(updateState);
    }

    private void updateState(float value)
    {
        if(value > 0 && value <= 1)
            slider.value = value;

        //TODO: отладка
        Debug.Log(value);
    }

}
