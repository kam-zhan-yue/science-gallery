using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class KeypadButtonPopupItem : PopupItem
{
    [SerializeField] private TMP_Text text;
    
    public Action<string> onPressed;

    public void OnButtonPressed()
    {
        onPressed?.Invoke(text.text);
    }
}
