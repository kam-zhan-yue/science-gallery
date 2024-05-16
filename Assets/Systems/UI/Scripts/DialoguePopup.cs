using System.Collections.Generic;
using DG.Tweening;
using Ink.Runtime;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePopup : Popup
{
    [Header("References")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text body;

    [Header("Parameters")] 
    [SerializeField] private float typeSpeed = 15f;

    private bool _textScrolling = false;
    private Tween _typeWriterTween;
    private DialoguePayload _data = new();

    protected override void InitPopup()
    {
        ServiceLocator.Instance.Get<IPopupService>().Register(this);
    }

    private void Start()
    {
        Messenger.Default.Subscribe<DialoguePayload>(UpdateDialogue);
        HidePopup();
    }
    
    // TODO This is extremely dirty but I don't have time
    private void Update()
    {
        if (!isShowing) return;
            
        bool choicesShowing = ServiceLocator.Instance.Get<IPopupService>().GetPopup<DialogueChoicePopup>().isShowing;
        if (!choicesShowing && Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            AdvanceDialogue();
        }
    }

    private void AdvanceDialogue()
    {
        // If there is text still scrolling, then kill the tween
        if (_textScrolling)
        {
            _typeWriterTween.Kill();
            _textScrolling = false;
        }
        // If not, check whether there are choices. If there are choices, then show
        else if(_data.choices.Count > 0)
        {
            DisplayChoices(_data.choices);
        }
        else
        {
            ServiceLocator.Instance.Get<IDialogueService>().Advance();
        }
    }


    private void UpdateDialogue(DialoguePayload payload)
    {
        _data = payload;
        if (_data.stop)
        {
            HidePopup();
        }
        else
        {
            if (!isShowing)
                ShowPopup();
            DisplayDialogue("Test Header", _data.body);
        }
    }

    private void DisplayDialogue(string header, string message)
    {
        Debug.Log($"Displaying Dialogue {header}: {message}");
        title.SetText(header);
        string text = string.Empty;
        float speed = typeSpeed;
        if (speed == 0f)
            speed = 1f;
        _textScrolling = true;
        _typeWriterTween = DOTween.To(() => text, x => text = x, message, message.Length / speed)
            .OnUpdate(() =>
            {
                body.SetText(text);
            })
            .OnComplete(() =>
            {
                _textScrolling = false;
            }).OnKill(() =>
            {
                _textScrolling = false;
                body.SetText(message);
            });
    }

    private void DisplayChoices(List<Choice> choices)
    {
        Debug.Log($"Displaying Choices {choices}");
        DialogueChoicePopup dialogueChoicePopup = ServiceLocator.Instance.Get<IPopupService>().GetPopup<DialogueChoicePopup>();
        dialogueChoicePopup.Init(choices);
        dialogueChoicePopup.ShowPopup();
    }
}