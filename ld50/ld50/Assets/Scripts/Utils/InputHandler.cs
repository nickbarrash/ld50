using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler: Singleton<InputHandler>
{
    public Camera mainCam;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Waypoints.Instance.AddWaypoint(MouseToWorldZeroed());
        }
    }

    public Vector3 MouseToWorldZeroed()
    {
        var mousePoint = MouseToWorld();
        return new Vector2(mousePoint.x, mousePoint.y);
    }

    private Vector3 MouseToWorld()
    {
        return mainCam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
    }
}