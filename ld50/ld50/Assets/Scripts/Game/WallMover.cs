using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WallMover : PausableRigidbody
{
    public Vector2 velocity;
    
    public float yMin;
    public float yMax;

    protected override void Start() {
        base.Start();

        rb.velocity = velocity;
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (Paused)
            return;

        if (transform.position.y > yMax) {
            transform.position = new Vector3(transform.position.x, yMin, transform.position.z);
        } else if (transform.position.y < yMin) {
            transform.position = new Vector3(transform.position.x, yMax, transform.position.z);
        }
    }
}
