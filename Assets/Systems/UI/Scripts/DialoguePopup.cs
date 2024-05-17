using System.Collections.Generic;
using DG.Tweening;
using Ink.Runtime;
using Kuroneko.AudioDelivery;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using MEC;
using SuperMaxim.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePopup : Popup
{
    [Header("References")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text body;
    [SerializeField] private RectTransform header;
    [SerializeField] private Image portrait;

    [Header("Parameters")] 
    [SerializeField] private float typeSpeed = 15f;

    private bool _textScrolling = false;
    private DialoguePayload _data = new();
    private CoroutineHandle _typewriterRoutine;

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
        ServiceLocator.Instance.Get<IAudioService>().Play("BUTTON");
        // If there is text still scrolling, then kill the tween
        if (_textScrolling)
        {
            _textScrolling = false;
            Timing.KillCoroutines(_typewriterRoutine);
            body.SetText(_data.body);
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
            header.gameObject.SetActiveFast(!string.IsNullOrEmpty(payload.title));
            portrait.gameObject.SetActiveFast(payload.portrait != null);
            portrait.sprite = payload.portrait;
            DisplayDialogue(_data.title, _data.body);
        }
    }

    private void DisplayDialogue(string header, string message)
    {
        Debug.Log($"Displaying Dialogue {header}: {message}");
        title.SetText(header);
        _textScrolling = true;
        _typewriterRoutine = Timing.RunCoroutine(TypewriterRoutine(message));
    }

    private IEnumerator<float> TypewriterRoutine(string message)
    {
        _textScrolling = true;
        body.text = string.Empty;
        bool richTag = false;
        for (int i = 0; i < message.Length; ++i)
        {
            body.text += message[i];
            if (message[i] == '<')
            {
                richTag = true;
            }
            else if (richTag && message[i] == '>')
            {
                richTag = false;
            }
            else if(!richTag)
            {
                yield return Timing.WaitForSeconds(1/typeSpeed);
            }
        }

        _textScrolling = false;
    }

    private void DisplayChoices(List<Choice> choices)
    {
        Debug.Log($"Displaying Choices {choices}");
        DialogueChoicePopup dialogueChoicePopup = ServiceLocator.Instance.Get<IPopupService>().GetPopup<DialogueChoicePopup>();
        dialogueChoicePopup.Init(choices);
        dialogueChoicePopup.ShowPopup();
    }
}