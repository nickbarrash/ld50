using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PausableRigidbody
{
    const int STUCK_TICKS = 100; //3 secs
    const float STUCK_DISTANCE = 0.25f;
    const float STUCK_THRUST_FACTOR = 2.5f;
    const float VELO_DRAW_LENGTH = 0.5f;

    private LineRenderer thrustDrawer;

    private Vector3 stuckPosition;
    private int stuckTick;

    public bool Stuck => Simulation.Instance.Ticks >= stuckTick + STUCK_TICKS;

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
            //veloNormal = Vector2.zero;
            return;
        }

        if ((transform.position - stuckPosition).magnitude >= STUCK_DISTANCE) {
            stuckTick = Simulation.Instance.Ticks;
            stuckPosition = transform.position;
        }

        var thrustNormal = (Vector2)(waypoint.transform.position - transform.position).normalized;
        var thrustOrthognal = new Vector2(thrustNormal.y, -thrustNormal.x);
        //if (Simulation.Instance.Ticks >= stuckTick + STUCK_TICKS) {
        //    // stuck, thrust orthogonally
        //    thrust = thrustOrthognal * Waypoints.Instance.Thrust * STUCK_THRUST_FACTOR;
        //} else {
            // normal thrust
            var thrustMiss = Vector2.Dot(thrustOrthognal, rb.velocity.normalized) * thrustOrthognal;

            var thrust = (thrustNormal - thrustMiss).normalized * Waypoints.Instance.Thrust;
        //}

        rb.AddForce(thrust);
    }

    public void SetThrust() {
        thrustDrawer.SetPosition(0, transform.position);

        var velo = Paused ? StoredVelocity : rb.velocity;
        if (velo.magnitude > 1)
            velo.Normalize();

        //var velo = rb.velocity.magnitude > 1 ? rb.velocity.normalized : rb.velocity;
        thrustDrawer.SetPosition(1, transform.position + ((Vector3)velo * VELO_DRAW_LENGTH));
    }
}
