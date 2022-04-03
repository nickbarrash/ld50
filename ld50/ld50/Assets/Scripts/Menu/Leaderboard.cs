using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public TMP_Text names;
    public TMP_Text scores;

    const int MAX_NAME_LENGTH = 18;
    const int SLOTS = 7;

    protected List<Score> latestScores;

    public List<Score> Scores;

    protected virtual string URL => "https://olhaytwbf1.execute-api.us-east-2.amazonaws.com/leaderboard/ld50-score";

    private void Start() {
        FetchLeaderboard(names != null && scores != null);
    }

    public void FetchLeaderboard(bool setUI)
    {
        try
        {
            Http.get(
                this,
                URL,
                (code, body) => {

                    var leaderboard = JsonUtility.FromJson<LeaderboardResponse>(body);
                    leaderboard.scores = leaderboard.scores.OrderByDescending(s => s.score).Take(SLOTS).ToList();

                    latestScores = leaderboard.scores;;

                    if (setUI)
                        SetLeaderboard(leaderboard);
                },
                new Dictionary<string, string> {}
            );
        }
        catch(Exception e)
        {
            Debug.LogError("Failed getting leaderboard");
            Debug.LogError(e);
        }
    }

    protected virtual void SetLeaderboard(LeaderboardResponse response) {
        names.text = string.Join("\n", response.scores.Select(s => s.name.Substring(0, Mathf.Min(s.name.Length, MAX_NAME_LENGTH))));
        scores.text = string.Join("\n", response.scores.Select(s => s.score));
    }

    public void Post(string name, float score, Action callback) {
        var payload = new ScorePost {
            name = name,
            score = score,
            epoch = Extensions.EpochSeconds().ToString()
        }.Payload();    

        try
        {
            Http.post(
                this,
                URL,
                payload,
                (code, body) => {
                    callback();
                }
            );
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed posting {payload} to leaderboard");
            Debug.LogError(e);
            callback(); // assuming we still want to do this (go back to home scene)
        }
    }
}
