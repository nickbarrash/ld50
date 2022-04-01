using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[Serializable]
public class Score
{
    public float score;
    public string name;
}

public class ScorePost
{
    public float score;
    public string name;
    public string epoch;

    public string Payload() {
        return Encode.Process(JsonUtility.ToJson(this));
    }
}


[Serializable]
public class LeaderboardResponse
{
    public List<Score> scores;
}


public class Leaderboard : MonoBehaviour
{
    const string URL = "https://olhaytwbf1.execute-api.us-east-2.amazonaws.com/leaderboard/ld50";

    private void Start() {
        GetLeaderboard();
        PostLeaderboard(new ScorePost {
            score = 456,
            name = "unity 2",
            epoch = "1234564"
        });
    }

    public void GetLeaderboard()
    {
        try
        {
            Http.get(
                this,
                URL,
                (code, body) => {
                    Debug.Log($"Leaderboard GET result: {body}");

                    var resp = JsonUtility.FromJson<LeaderboardResponse>(body);
                    foreach (var score in resp.scores){
                        Debug.Log($"{score.name}: {score.score}");
                    }
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

    public void PostLeaderboard(ScorePost score)
    {
        PostLeaderboard(score.Payload());
    }

    private void PostLeaderboard(string payload)
    {
        try
        {
            Http.post(
                this,
                URL,
                payload,
                (code, body) => {
                    Debug.Log($"Leaderboard POST result: {body}");
                    //RefreshLeaderboard();
                }
            );
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed posting {payload} to leaderboard");
            Debug.LogError(e);
        }
    }
}
