using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgresBar : MonoBehaviour, IProgressBar
{
    private Slider slider;
    private float configTime;

    [SerializeField]public float fillSpeed;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void updateProgress(float newProgress)
    {                
        slider.value = 1 - (newProgress / configTime); 
    }

    public void dropProgress()
    {
        // TODO: Выяснить почему не работает
        slider.value = 0.1f;
    }

    public void setConfig(float _time)
    {
        configTime = _time;
    }
}