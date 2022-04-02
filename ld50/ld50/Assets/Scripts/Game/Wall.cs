using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Collider2D collider;
    private int passableTick = 0;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate() {
        //if (Simulation.Instance.Ticks)
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent<Ball>(out _)) {
            SetSolid(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out Ball ball) && ball.Stuck) {
            SetSolid(false);
        }
    }

    public void SetSolid(bool isSolid) {
        collider.isTrigger = !isSolid;
        spriteRenderer.color = isSolid ? Config.Instance.WALL_SOLID : Config.Instance.WALL_PASSABLE;

        if (!isSolid) {
            passableTick = Simulation.Instance.Ticks;
        }
    }
}
