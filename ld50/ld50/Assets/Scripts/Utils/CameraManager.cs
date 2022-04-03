using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera mainCam;
    
    const float widthRatio= 2000;
    const float heightRatio = 1110;

    const float orthogHeight = 5;
    const float orthogWidth = widthRatio / heightRatio * orthogHeight;

    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;

        mainCam.orthographicSize = ((float)Screen.height / Screen.width) / (orthogHeight / orthogWidth) * orthogHeight;

        Debug.Log($"{mainCam.orthographicSize}");
    }
}
