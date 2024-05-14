using System;
using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;

public class KeypadPopup : Popup
{
    [SerializeField] private TMP_Text display;
    
    [NonSerialized, ReadOnly, ShowInInspector] 
    private KeypadButtonPopupItem[] _buttons = Array.Empty<KeypadButtonPopupItem>();

    protected override void InitPopup()
    {
        Debug.Log("Register KeypadPopup");
        ServiceLocator.Instance.Get<IPopupService>().Register(this);
        _buttons = GetComponentsInChildren<KeypadButtonPopupItem>();
        display.SetText(string.Empty);
    }

    private void Start()
    {
        for (int i = 0; i < _buttons.Length; ++i)
        {
            _buttons[i].onPressed += OnPressed;
        }
    }

    private void OnPressed(string value)
    {
        if(display.text.Length < 4)
            display.text += value;
    }

    public void Submit()
    {
        CodePayload codePayload = new CodePayload { code = display.text };
        Messenger.Default.Publish(codePayload);
        HidePopup();
    }

    public void Delete()
    {
        if(display.text.Length > 0)
            display.SetText(display.text.Remove(display.text.Length - 1));
    }

    public override void CloseButtonClicked()
    {
        base.CloseButtonClicked();
        HidePopup();
    }
}
