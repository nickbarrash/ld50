using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : Singleton<Simulation>
{
    public const int TICKS_SECOND = 50;
    public const int TICKS_MINUTE = 50 * 60;

    private int simulationTicks = 0;

    public int Ticks => simulationTicks;

    public bool Simulating => Waypoints.Instance.Waypoint != null;

    private void FixedUpdate() {
        if (Simulating)
            simulationTicks++;

        //if (Waypoints.Instance.Waypoint == null) {
        //    //Physics.autoSimulation = false;
        //    //Time.timeScale = 0;
        //} else {
        //    //Physics.autoSimulation = true;
        //    //Time.timeScale = 1;
        //    simulationTicks++;
        //}
    }
}
