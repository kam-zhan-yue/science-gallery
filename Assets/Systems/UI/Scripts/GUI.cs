using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUI : Popup
{
    [SerializeField] private SmartButton keypadButton;

    protected override void InitPopup()
    {
        ServiceLocator.Instance.Get<IPopupService>().Register(this);
        ShowPopup();
    }

    private void Start()
    {
        keypadButton.onPointerClick += ShowKeypad;
        KeypadPopup keypadPopup = ServiceLocator.Instance.Get<IPopupService>().GetPopup<KeypadPopup>();
        keypadPopup.onCloseButtonClicked += ShowKeypadButton;
        Messenger.Default.Subscribe<DialoguePayload>(UpdateDialogue);
    }

    private void UpdateDialogue(DialoguePayload dialoguePayload)
    {
        if(dialoguePayload.stop)
            ShowPopup();
        else if(isShowing)
            HidePopup();
    }
    
    private void ShowKeypad(PointerEventData pointerEventData)
    {
        ServiceLocator.Instance.Get<IPopupService>().ShowPopup<KeypadPopup>();
        keypadButton.gameObject.SetActiveFast(false);
    }

    private void ShowKeypadButton(Popup popup)
    {
        Debug.Log("Show Keypad Button");
        keypadButton.gameObject.SetActiveFast(true);
    }
}
