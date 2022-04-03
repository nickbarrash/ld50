using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputName : MonoBehaviour
{
    public Button buttonHome;
    public TMP_InputField inputName;

    public void OnChanged() {
        buttonHome.interactable = inputName.text.Length > 0;
    } 
}
