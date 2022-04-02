using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scorekeeper : Singleton<Scorekeeper>
{
    const int INITIAL_SCORE = 100;
    const int COMBO_FALLOFF_TICKS = Simulation.TICKS_SECOND * 5;
    const int DECREMENT_POINTS_PER_SCORE = 50;

    // Score variables
    private int settledScoreInt;

    private int decrementPoints = 0;
    private int decrementPointsPerTick = 1;

    private int activeComboScoreInt;
    private int activeComboCountInt;
    private int activeComboTicks;
    private bool scoreChanged = false;

    // GUI
    public TMP_Text labelScore;
    public TMP_Text labelComboScore;
    public TMP_Text labelComboCount;
    public Image comboBar;

    private RectTransform comboBarRectTransform;
    private float comboAffordanceWidth;

    // Properties
    public bool ComboActive  => ComboValue > 0;

    public int ComboScore => ComboValue * ComboCount;

    private int ComboCount {
        get => activeComboCountInt;
        set {
            activeComboCountInt = value;
            scoreChanged = true;   
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

    public int Score => SettledScore + ComboScore;

    private void Awake() {
        comboBarRectTransform = comboBar.GetComponent<RectTransform>();
        comboAffordanceWidth = comboBarRectTransform.sizeDelta.x;
    }

    private void Start() {
        SettledScore = INITIAL_SCORE;
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

        if (scoreChanged) {
            labelComboScore.text = ComboScore.ToString();
            labelComboCount.text = $"x{ComboCount}";
            labelScore.text = Score.ToString();

            scoreChanged = false;
        }
    }

    private void FixedUpdate() {
        if (Simulation.Instance.Simulating) {
            decrementPoints += decrementPointsPerTick;
            if (decrementPoints >= DECREMENT_POINTS_PER_SCORE) {
                var lostPoints = decrementPoints / DECREMENT_POINTS_PER_SCORE;
                SettledScore -= lostPoints;
                decrementPoints -= lostPoints * DECREMENT_POINTS_PER_SCORE;
            }

            if (ComboActive) {
                activeComboTicks -= ComboCount;
                if (activeComboTicks <= 0)
                    SettleCombo();
            }
        }
    }

    public void ScoreToken(Token token) {
        ComboValue += token.value;
        ComboCount++;
        activeComboTicks = COMBO_FALLOFF_TICKS;
    }

    public void SettleCombo() {
        SettledScore += ComboScore;

        ComboValue = 0;
        ComboCount = 0;
    }
}
