using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeypadButtonPopupItem : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    
    public Action<string> onPressed;

    public void OnButtonPressed()
    {
        onPressed?.Invoke(text.text);
    }
}
