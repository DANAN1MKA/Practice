using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgresBar : MonoBehaviour, IProgressBar
{
    private Slider slider;

    [SerializeField]public float fillSpeed; //0,75f
    private float targetProgress = 1;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    private void incrementProgress(float newProgress)
    {
        slider.value += newProgress;
    }

    public void updateProgress(float newProgress)
    {                       //Алерт!!!!! Залупа Алерт!!!!!
        slider.value = 1 - (newProgress / 5); // <- такого не должно быть (про пятерку идет речь)
    }

    public void dropProgress()
    {
        slider.value = 0.1f;
    }
}

public interface IProgressBar 
{
    void updateProgress(float newProgress);

    void dropProgress();
}

