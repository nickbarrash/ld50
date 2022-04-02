using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private LineRenderer thrustDrawer;
    private Vector2 thrust;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        thrustDrawer = GetComponent<LineRenderer>();
    }

    public void Update() {
        SetThrust();
    }

    private void FixedUpdate() {
        var waypoint = Waypoints.Instance.Waypoint;
        if (waypoint == null) {
            thrust = Vector2.zero;
            return;
        }

        var thrustNormal = (Vector2)(waypoint.transform.position - transform.position).normalized;
        var thrustOrthognal = new Vector2(thrustNormal.y, -thrustNormal.x);
        var thrustMiss = Vector2.Dot(thrustOrthognal, rb.velocity.normalized) * thrustOrthognal;

        thrust = (thrustNormal - thrustMiss).normalized * Waypoints.Instance.Thrust;

        rb.AddForce(thrust);
    }

    public void SetThrust() {
        thrustDrawer.SetPosition(0, transform.position);
        thrustDrawer.SetPosition(1, transform.position + (Vector3)thrust.normalized);
    }
}
