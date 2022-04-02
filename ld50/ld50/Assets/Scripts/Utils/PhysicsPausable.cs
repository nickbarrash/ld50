using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pausable : MonoBehaviour
{
    private bool paused;

    public bool Paused => paused;

    public virtual void Pause() {
        paused = true;
    }

    public virtual void Resume() {
        paused =  false;
    }

    protected virtual void FixedUpdate() {
        var simulating = Simulation.Instance.Simulating;
        if (simulating && Paused) {
            Resume();
        } else if (!simulating && !Paused) {
            Pause();
        }
    }
}

public abstract class PausableRigidbody : Pausable {
    protected Rigidbody2D rb;

    protected Vector2 rbPosition;
    protected Vector2 rbVelocity;
    protected float rbRotation;
    protected float rbAngularVelocity;

    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Pause() {
        base.Pause();

        rbPosition = rb.position;
        rbVelocity = rb.velocity;
        rbRotation = rb.rotation;
        rbAngularVelocity = rb.angularVelocity;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    public override void Resume() {
        base.Resume();

        rb.velocity = rbVelocity;
        rb.angularVelocity = rbAngularVelocity;
    }
}
