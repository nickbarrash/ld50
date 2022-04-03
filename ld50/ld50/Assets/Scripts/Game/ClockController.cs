using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public Material clockBackgroundStartMaterial;

    private void Update() {
        setClock((float) Scorekeeper.Instance.TicksUntilDecrementIncrease / (float)Scorekeeper.DECREMENT_TIME_TICKS);
    }

    public void setClock(float newProgress) {
        newProgress = Mathf.Clamp(newProgress, 0, 1);
        clockBackgroundStartMaterial.SetFloat("_WipeProgress", 1 - newProgress);
        //clockHand.transform.rotation = initialClockHandRotation;
        //clockHand.transform.Rotate(0, 0, Mathf.Lerp(-360, 0, newProgress));
    }
}
