using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : Singleton<GameOverMenu>
{
    public GameObject highScorePanel;

    public TMP_Text labelScore;
    public TMP_Text labelTime;
    public TMP_Text labelHomeButton;
    public TMP_InputField inputName;
    public Button buttonHome;

    public Leaderboard leaderboardTime;
    public Leaderboard leaderboardScores;

    private bool scoreHighScore = false;
    private bool timeHighScore = false;

    public void SetupPanel() {
        labelScore.text = Scorekeeper.Instance.AccumulatedScore.ToString();
        var scores = leaderboardScores.Scores;
        scoreHighScore = scores != null && (scores.Count == 0 || Scorekeeper.Instance.AccumulatedScore > scores.Last().score);

        labelTime.text = LeaderboardTime.FormatSecondsTime(Simulation.Instance.Seconds);
        var times = leaderboardTime.Scores;
        timeHighScore = times != null && (times.Count == 0 || Simulation.Instance.Seconds > scores.Last().score);

        bool showHighScore = scoreHighScore || timeHighScore;

        highScorePanel.SetActive(scoreHighScore || timeHighScore);

        labelHomeButton.text = showHighScore ? "Submit" : "Home";
        buttonHome.interactable = !showHighScore;
    }

    public void Home() {
        if (scoreHighScore || timeHighScore) {
            if (timeHighScore) {
                // WebGL is single threaded so we dont need to lock maybe?
                leaderboardTime.Post(inputName.text, Simulation.Instance.Seconds, () => {
                    scoreHighScore = false;
                    if (!scoreHighScore && !timeHighScore)
                        SceneManager.LoadScene("Lobby");
                });
            }

            if (scoreHighScore) {
                leaderboardScores.Post(inputName.text, Scorekeeper.Instance.AccumulatedScore, () => {
                    timeHighScore = false;
                    if (!scoreHighScore && !timeHighScore)
                        SceneManager.LoadScene("Lobby");
                });
            }

            highScorePanel.SetActive(false);
            buttonHome.interactable = false;
        } else {
            SceneManager.LoadScene("Lobby");
        }
    }
}
