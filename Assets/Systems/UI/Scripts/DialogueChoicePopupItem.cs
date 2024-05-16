using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using Kuroneko.UtilityDelivery;
using TMPro;
using UnityEngine;

public class DialogueChoicePopupItem : MonoBehaviour
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
    }
}
