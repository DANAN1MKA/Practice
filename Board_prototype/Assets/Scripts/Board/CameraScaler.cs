using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    float _padding = 0.5f;
    private void Start()
    {
        Scale(7, 8, _padding);
    }

    void Scale(int width, int height, float boundSize)
    {
        float aspectRatio = (float)Screen.width / Screen.height;

        float verticalSize = (float)height / 2f + boundSize;

        float horizontalSize = ((float)width / 2f + boundSize) / aspectRatio;
        float orthoSize = horizontalSize;
        Camera.main.orthographicSize = orthoSize;
        //Camera.main.transform.position = new Vector3((float)(width - 1) / 2f, orthoSize - 2f, -10f);
    }

}
