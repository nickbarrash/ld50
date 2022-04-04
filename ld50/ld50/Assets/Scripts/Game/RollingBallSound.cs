using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBallSound : MonoBehaviour
{
    public AudioClip rollingSound;

    public Rigidbody2D rb;
    private AudioSource source;

    private void Awake() {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = rollingSound;
        source.loop = true;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        source.Play();
        source.volume = 0f;
    }

    private void Update() {
        if (Scorekeeper.Instance.GameOver || !Simulation.Instance.Simulating) {
            source.volume = 0;
            return;
        }

        source.volume = Mathf.Clamp01(rb.velocity.magnitude / 10);
    }


}
