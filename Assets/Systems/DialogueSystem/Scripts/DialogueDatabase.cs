using System;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogueDatabase")]
public class DialogueDatabase : ScriptableObject
{
    [Serializable]
    public class DialogueData
    {
        public string id;
        public TextAsset text;
    }

    [TableList] public DialogueData[] dialogue;

    public TextAsset GetText(string id)
    {
        for (int i = 0; i < dialogue.Length; ++i)
        {
            if (dialogue[i].id == id)
                return dialogue[i].text;
        }

        return null;
    }

    [Button]
    private void StartStory(string id)
    {
        ServiceLocator.Instance.Get<IDialogueService>().Play(id);
    }
}