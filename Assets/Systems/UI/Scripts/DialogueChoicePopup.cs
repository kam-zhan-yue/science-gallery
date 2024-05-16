using System.Collections.Generic;
using Ink.Runtime;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoicePopup : Popup
{
    [SerializeField] private LayoutGroup choiceHolder;
    [SerializeField] private DialogueChoicePopupItem sampleChoicePopupItem;

    private readonly List<DialogueChoicePopupItem> _choices = new();

    protected override void InitPopup()
    {
        ServiceLocator.Instance.Get<IPopupService>().Register(this);
    }

    private void Start()
    {
        HidePopup();
    }
    
    public void Init(List<Choice> choices)
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