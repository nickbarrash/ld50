using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    bool gameOverAck = false;

    public GameObject hud;
    public GameObject gameOverPanel;


    private void Start() {
        gameOverPanel.SetActive(false);
        hud.SetActive(true);
    }

    public void GameOver() {
        if (gameOverAck)
            return;

        gameOverAck = true;

        hud.SetActive(false);
        gameOverPanel.SetActive(true);

        GameOverMenu.Instance.SetupPanel();
    }
}
