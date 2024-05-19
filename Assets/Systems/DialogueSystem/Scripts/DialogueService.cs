using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using Kuroneko.AudioDelivery;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using SuperMaxim.Messaging;
using UnityEngine;

public class DialogueService : MonoBehaviour, IDialogueService
{
    [SerializeField] private DialogueDatabase dialogueDatabase;
    [SerializeField] private CharacterDatabase characterDatabase;
    private Story _currentStory;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IDialogueService>(this);
    }
    
    public void Play(string id)
    {
        ServiceLocator.Instance.Get<IAudioService>().Play("MAIN");
        _currentStory = new Story(dialogueDatabase.GetText(id)?.text);
        Advance();
    }

    [Button]
    public void Advance()
    {
        if (_currentStory.canContinue)
        {
            Continue();
        }
        // If the story cannot continue and there are no more choices, then the story is ended
        else if(!_currentStory.canContinue && _currentStory.currentChoices.Count == 0)
        {
            Stop();
        }
    }

    private void Continue()
    {
        string message = _currentStory.Continue();
        string id, body;
        // Split the string at the first instance of ':'
        string[] parts = message.Split(new char[] { ':' }, 2);
        if (parts.Length < 2)
        {
            id = string.Empty;
            body = message;
        }
        else
        {
            id = parts[0].Trim(); 
            body = parts[1].Trim();
        }
        Messenger.Default.Publish(CreatePayload(id, body));
    }

    private DialoguePayload CreatePayload(string id, string body)
    {
        DialoguePayload payload = new();
        if (characterDatabase.TryGetData(id, out CharacterDatabase.CharacterData data))
        {
            payload.title = data.name;
            payload.body = body;
            payload.portrait = data.portrait;
        }
        else
        {
            payload.title = id;
            payload.body = body;
        }
        payload.choices = _currentStory.currentChoices;
        return payload;
    }

    private void Stop()
    {
        DialoguePayload payload = new DialoguePayload
        {
            stop = true,
        };
        Messenger.Default.Publish(payload);
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
