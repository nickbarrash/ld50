using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : Singleton<Simulation>
{
    public const int TICKS_SECOND = 50;
    public const int TICKS_MINUTE = 50 * 60;

    private int simulationTicks = 0;

    public int Ticks => simulationTicks;

    private void FixedUpdate() {
        simulationTicks++;
    }
}
