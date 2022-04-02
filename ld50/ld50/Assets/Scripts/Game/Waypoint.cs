using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public void Deactivate() {
        gameObject.SetActive(false);
    }

    public void Activate() {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Waypoints.Instance.NextWaypoint();
    }
}
