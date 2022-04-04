using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    bool gameOverAck = false;

    public GameObject startPanel;
    public GameObject hud;
    public GameObject gameOverPanel;

    public TMP_Text labelHighScoreTitle;
    public TMP_Text labelScoreTitle;

    private void Start() {
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        hud.SetActive(true);
    }

    private void Update() {
        if (hud.activeSelf && Time.frameCount % 30 == 0 && !labelHighScoreTitle.gameObject.activeSelf) {
            bool highScore = GameOverMenu.Instance.leaderboardScores.IsHighScore(Scorekeeper.Instance.AccumulatedScore)
                || GameOverMenu.Instance.leaderboardTime.IsHighScore(Simulation.Instance.Seconds);

            if (highScore) {
                labelScoreTitle.gameObject.SetActive(false);
                labelHighScoreTitle.gameObject.SetActive(true);
            }
        }
    }

    public void GameOver() {
        if (gameOverAck)
            return;

        gameOverAck = true;

        startPanel.SetActive(false);
        hud.SetActive(false);
        gameOverPanel.SetActive(true);

        GameOverMenu.Instance.SetupPanel();
    }
}
