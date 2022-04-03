using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardTime : Leaderboard
{
    protected override string URL => "https://olhaytwbf1.execute-api.us-east-2.amazonaws.com/leaderboard/ld50-time";

    public static string FormatSecondsTime(float time) {
        var span = TimeSpan.FromSeconds(time);
        return time > 60 * 60 ? span.ToString() : span.ToString(@"mm\:ss");
    }

    protected override void SetLeaderboard(LeaderboardResponse response) {
        base.SetLeaderboard(response);

        scores.text = string.Join("\n", response.scores.Select(s => FormatSecondsTime(s.score)));
    }
}
