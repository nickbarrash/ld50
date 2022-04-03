using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Simulation : Singleton<Simulation>
{
    public const int TICKS_SECOND = 50;
    public const int TICKS_MINUTE = 50 * 60;

    private int simulationTicks = 0;

    public int Ticks => simulationTicks;

    public bool Simulating => Waypoints.Instance.Waypoint != null && !Scorekeeper.Instance.GameOver;

    public float Seconds => simulationTicks / TICKS_SECOND;

    private void FixedUpdate() {
        if (Simulating)
            simulationTicks++;
    }
}
