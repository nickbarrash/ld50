using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Token : MonoBehaviour
{
    public int value;
    
    public TMP_Text labelValue;

    public void SetInfo(TokenSpawnInfo info) {
        value = info.value;
        labelValue.text = info.value.ToString();
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        Scorekeeper.Instance.ScoreToken(this);
        Destroy(gameObject);
    }
}
