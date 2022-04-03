using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Token : MonoBehaviour
{
    const float OVERLAP_PERTURB_FACTOR = 0.01f;

    public int value;
    public bool absorbed;
    
    public TokenSpawner spawner;
    public TMP_Text labelValue;

    private static float perturbAngleInt;
    private static float PerturbAngle {
        get {
            perturbAngleInt += 113f; // some prime
            return perturbAngleInt;
        }
    }

    public void SetInfo(TokenSpawnInfo info) {
        SetValue(info.value);
    }

    private void SetValue(int value) {
        this.value = value;
        labelValue.text = Extensions.ShortenedValue(this.value);
    }

    //private void OnTriggerStay2D(Collider2D collision) {
    //    if (!collision.gameObject.TryGetComponent<Token>(out _))
    //        return;

    //    var otherTransform = collision.gameObject.transform;
    //    if (otherTransform.position == transform.position) {
    //        transform.position += Vector3.up.Rotate(PerturbAngle) * 0.0001f;
    //    }

    //    transform.position += (transform.position - otherTransform.position).normalized * OVERLAP_PERTURB_FACTOR;
    //}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent<Ball>(out _)) {
            // ball scored
            Scorekeeper.Instance.ScoreToken(this);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.TryGetComponent(out Token token)) {
            // absorb
            if (!absorbed && !token.absorbed) {
                token.absorbed = true;
                SetValue(value + token.value);
                Destroy(token.gameObject);
                return;
            }
        }
    }
}
