using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scorekeeper : Singleton<Scorekeeper>
{
    const int INITIAL_SCORE = 100;
    const float COMBO_FALLOFF_TICKS = Simulation.TICKS_SECOND * 6;
    
    const int INITIAL_DECREMENT_POINTS = 20;
    const int DECREMENT_POINTS_PER_SCORE = 250;

    const int DECREMENT_RATE_PER_WAYPOINT = 10;

    public const int DECREMENT_TIME_TICKS = 500;
    const int DECREMENT_TIME_RATE = 20;
    const int DECREMENT_TIME_RATE_INCREASE_PER_MINUTE = 10;

    // Score variables
    private int settledScoreInt;
    private int accumulatedScoreInt;

    private int decrementPoints = 0;
    private int decrementPointsPerTickInt = INITIAL_DECREMENT_POINTS;

    private int activeComboScoreInt;
    private int activeComboCountInt;
    private float activeComboTicks;
    private bool scoreChanged = false;

    // GUI
    public TMP_Text labelScore;
    public TMP_Text labelDecrementScore;
    public TMP_Text labelComboScore;
    public TMP_Text labelComboCount;
    public TMP_Text labelAccumulatedScore;
    public TMP_Text labelTime;
    public Image comboBar;

    private RectTransform comboBarRectTransform;
    private float comboAffordanceWidth;

    // Properties
    public bool ComboActive  => ComboValue > 0;

    public bool GameOver => Score < 0;

    public int ComboScore => ComboValue * ComboCount;

    private int DecrementPointsPerTick {
        get => decrementPointsPerTickInt;
        set {
            decrementPointsPerTickInt = value;
            scoreChanged = true;
        }
    }

    private int ComboCount {
        get => activeComboCountInt;
        set {
            activeComboCountInt = value;
            scoreChanged = true;   
        }
    }

    private float ComboTimeIncrement {
        get {
            var count = ComboCount;
            if (count <= 3)
                return count;

            var increment = 3f;
            count -= 3;

            if (count <= 3)
                return increment + count * 0.5f;

            increment += 3 * 0.5f;
            count -= 3;

            return increment + count * 0.25f;
        }
    }

    private int ComboValue {
        get => activeComboScoreInt;
        set {
            activeComboScoreInt = value;
            scoreChanged = true;   
        }
    }

    private int SettledScore {
        get => settledScoreInt;
        set {
            settledScoreInt = value;
            scoreChanged = true;
        }
    }

    public int AccumulatedScore {
        get => accumulatedScoreInt;
        set {
            accumulatedScoreInt = value;
            scoreChanged = true;
        }
    }

    public int TicksUntilDecrementIncrease => DECREMENT_TIME_TICKS - Simulation.Instance.Ticks % DECREMENT_TIME_TICKS;

    public int Score => SettledScore + ComboScore;

    protected override void Awake() {
        base.Awake();

        comboBarRectTransform = comboBar.GetComponent<RectTransform>();
        comboAffordanceWidth = comboBarRectTransform.sizeDelta.x;
    }

    private void Start() {
        SettledScore = INITIAL_SCORE;
        AccumulatedScore = INITIAL_SCORE;
    }

    private void Update() {
        var comboActive = ComboActive;
        labelComboScore.gameObject.SetActive(comboActive);
        labelComboCount.gameObject.SetActive(comboActive);
        comboBar.gameObject.SetActive(comboActive);

        if (comboActive) {
            comboBarRectTransform.sizeDelta = new Vector2(
                ((float)activeComboTicks / (float)COMBO_FALLOFF_TICKS) * comboAffordanceWidth,
                comboBarRectTransform.sizeDelta.y
            );
        }

        labelTime.text = LeaderboardTime.FormatSecondsTime(Simulation.Instance.Seconds);

        if (scoreChanged) {
            labelComboScore.text = ComboValue.ToString();
            labelComboCount.text = $"x{ComboCount}";

            var score = Score;
            var sign = score < 0 ? "-" : "+";
            labelScore.text = $"{sign}{Score}";

            labelAccumulatedScore.text = $"{AccumulatedScore}";
            labelDecrementScore.text = "-" + ((float)(DecrementPointsPerTick * Simulation.TICKS_SECOND) / (float) DECREMENT_POINTS_PER_SCORE).ToString("R2") + "/second";

            scoreChanged = false;
        }
    }

    private void FixedUpdate() {
        if (GameOver) {
            SettleCombo();
            GameManager.Instance.GameOver();
        }

        if (Simulation.Instance.Simulating) {
            if (Simulation.Instance.Ticks % DECREMENT_TIME_TICKS == 0 && Simulation.Instance.Ticks > 0) {
                DecrementPointsPerTick += DECREMENT_TIME_RATE + (((int)Simulation.Instance.Seconds / 60) * DECREMENT_TIME_RATE_INCREASE_PER_MINUTE);
                SoundManager.Instance.Play(SoundManager.DECREMENT);
            }

            decrementPoints += DecrementPointsPerTick;
            if (decrementPoints >= DECREMENT_POINTS_PER_SCORE) {
                var lostPoints = decrementPoints / DECREMENT_POINTS_PER_SCORE;
                SettledScore -= lostPoints;
                decrementPoints -= lostPoints * DECREMENT_POINTS_PER_SCORE;
            }

            if (ComboActive) {
                activeComboTicks -= ComboTimeIncrement;
                if (activeComboTicks <= 0)
                    SettleCombo();
            }
        }
    }

    public void ReachWaypoint() {
        DecrementPointsPerTick += DECREMENT_RATE_PER_WAYPOINT;
    }

    public void ScoreToken(Token token) {
        ComboValue += token.value;
        ComboCount++;
        activeComboTicks = COMBO_FALLOFF_TICKS;
        SoundManager.Instance.Play(SoundManager.TOKEN, 0.9f + 0.1f * ComboCount);
    }

    public void SettleCombo() {
        SettledScore += ComboScore;
        AccumulatedScore += ComboScore;

        ComboValue = 0;
        ComboCount = 0;

        SoundManager.Instance.Play(SoundManager.COMBO);
    }
}
