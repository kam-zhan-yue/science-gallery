using System.Collections.Generic;
using Ink.Runtime;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePopup : Popup
{
    [SerializeField] private TMP_Text body;
    [SerializeField] private LayoutGroup choiceHolder;
    [SerializeField] private DialogueChoicePopupItem sampleChoicePopupItem;

    private List<DialogueChoicePopupItem> _choices = new();

    protected override void InitPopup()
    {
        ServiceLocator.Instance.Get<IPopupService>().Register(this);
    }

    private void Start()
    {
        Messenger.Default.Subscribe<DialoguePayload>(UpdateDialogue);
        HidePopup();
    }

    private void UpdateDialogue(DialoguePayload payload)
    {
        if (payload.stop)
        {
            HidePopup();
        }
        else
        {
            if (!isShowing)
                ShowPopup();
            body.SetText(payload.body);
            DisplayChoices(payload.choices);
        }
    }

    private void DisplayChoices(List<Choice> choices)
    {
        int numToSpawn = choices.Count - _choices.Count;
        if (numToSpawn > 0)
        {
            sampleChoicePopupItem.gameObject.SetActiveFast(true);
            for (int i = 0; i < numToSpawn; ++i)
            {
                DialogueChoicePopupItem choice = Instantiate(sampleChoicePopupItem, choiceHolder.transform);
                _choices.Add(choice);
            }
        }
        sampleChoicePopupItem.gameObject.SetActiveFast(false);

        for (int i = 0; i < _choices.Count; ++i)
            _choices[i].gameObject.SetActiveFast(false);
        
        for (int i = 0; i < choices.Count; ++i)
        {
            _choices[i].gameObject.SetActiveFast(true);
            _choices[i].Init(choices[i]);
        }
    }
}