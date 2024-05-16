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

    private void Awake()
    {
        ServiceLocator.Instance.Register<IDialogueService>(this);
    }
    
    public void Play(string id)
    {
        _currentStory = new Story(dialogueDatabase.GetText(id)?.text);
        Advance();
    }

    [Button]
    public void Advance()
    {
        if (_currentStory.canContinue)
        {
             DialoguePayload dialoguePayload = new DialoguePayload
             {
                 body = _currentStory.Continue(),
                 choices = _currentStory.currentChoices,
                 stop = false,
             };
             Messenger.Default.Publish(dialoguePayload);
        }
        // If the story cannot continue and there are no more choices, then the story is ended
        else if(!_currentStory.canContinue && _currentStory.currentChoices.Count == 0)
        {
            DialoguePayload dialoguePayload = new DialoguePayload
            {
                stop = true,
            };
            Messenger.Default.Publish(dialoguePayload);
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
