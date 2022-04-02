using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PausableRigidbody
{
    private LineRenderer thrustDrawer;
    private Vector2 thrust;

    protected override void Start() {
        base.Start();    

        thrustDrawer = GetComponent<LineRenderer>();
    }

    public void Update() {
        SetThrust();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (Paused)
            return;

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
