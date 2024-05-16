using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    [Serializable]
    public class CharacterData
    {
        public string id;
        public string name;
        public Sprite portrait;
    }

    [TableList] public CharacterData[] characters;

    public bool TryGetData(string id, out CharacterData data)
    {
        for (int i = 0; i < characters.Length; ++i)
        {
            if (characters[i].id == id)
            {
                data = characters[i];
                return true;
            }
        }

        data = null;
        return false;
    }
}