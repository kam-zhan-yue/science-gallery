using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using SuperMaxim.Messaging;
using UnityEngine;

public class DialogueService : MonoBehaviour, IDialogueService
{
    [SerializeField] private DialogueDatabase dialogueDatabase;
    private Story _currentStory;
    private bool _playing = false;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IDialogueService>(this);
    }

    private void Update()
    {
        if (_playing && Input.GetMouseButtonDown(0))
        {
            Advance();
        }
    }

    public void Play(string id)
    {
        _currentStory = new Story(dialogueDatabase.GetText(id)?.text);
        _playing = true;
        Advance();
    }

    [Button]
    private void Advance()
    {
        if (_currentStory.canContinue && _currentStory.currentChoices.Count == 0)
        {
            Debug.Log($"Advance {_currentStory.currentChoices.Count}");
             DialoguePayload dialoguePayload = new DialoguePayload
             {
                 body = _currentStory.Continue(),
                 choices = _currentStory.currentChoices,
                 stop = false,
             };
             Messenger.Default.Publish(dialoguePayload);
        }
        else if(!_currentStory.canContinue)
        {
            Debug.Log("Stop playing");
            DialoguePayload dialoguePayload = new DialoguePayload
            {
                stop = true,
            };
            Messenger.Default.Publish(dialoguePayload);
            _playing = false;
        }
    }

    public void Select(int choiceIndex)
    {
        if (choiceIndex >= _currentStory.currentChoices.Count)
        {
            Debug.LogError($"Invalid choice index {choiceIndex} for current story!");
            return;
        }
        
        _currentStory.ChooseChoiceIndex(choiceIndex);
        Advance();
    }
}
