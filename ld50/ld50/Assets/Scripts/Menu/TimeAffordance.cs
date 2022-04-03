using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAffordance : MonoBehaviour
{
    public GameObject playTexture;
    public GameObject pauseTexture;

    public void Update() {
        if (playTexture.activeSelf != Simulation.Instance.Simulating)
            SetPlay(Simulation.Instance.Simulating);
    }

    public void SetPlay(bool isPlaying) {
        playTexture.SetActive(isPlaying);
        pauseTexture.SetActive(!isPlaying);
    }

}
