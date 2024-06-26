using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using Kuroneko.AudioDelivery;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using TMPro;
using UnityEngine;

public class DialogueChoicePopupItem : PopupItem
{
    [SerializeField] private TMP_Text text;
    
    private Choice _choice = new();
    
    public void Init(Choice choice)
    {
        _choice = choice;
        text.SetText(choice.text);
    }

    public void Select()
    {
        ServiceLocator.Instance.Get<IDialogueService>().Select(_choice.index);
        ServiceLocator.Instance.Get<IPopupService>().HidePopup<DialogueChoicePopup>();
        ServiceLocator.Instance.Get<IAudioService>().Play("BUTTON");
    }
}
