using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUI : Popup
{
    [SerializeField] private SmartButton keypadButton;

    protected override void InitPopup()
    {
        Debug.Log("Register GUI");
        ServiceLocator.Instance.Get<IPopupService>().Register(this);
    }

    private void Start()
    {
        keypadButton.onPointerClick += ShowKeypad;
        KeypadPopup keypadPopup = ServiceLocator.Instance.Get<IPopupService>().GetPopup<KeypadPopup>();
        keypadPopup.onCloseButtonClicked += ShowKeypadButton;
    }
    
    private void ShowKeypad(PointerEventData pointerEventData)
    {
        ServiceLocator.Instance.Get<IPopupService>().ShowPopup<KeypadPopup>();
        keypadButton.gameObject.SetActiveFast(false);
    }

    private void ShowKeypadButton(Popup popup)
    {
        keypadButton.gameObject.SetActiveFast(true);
    }
}
