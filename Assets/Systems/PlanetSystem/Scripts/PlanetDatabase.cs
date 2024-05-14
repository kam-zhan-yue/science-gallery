using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlanetDatabase")]
public class PlanetDatabase : ScriptableObject
{
    [Serializable]
    public class PlanetData
    {
        public string id;
        public string code;
        public string dialogueId;
        public Planet planet;
    }

    [TableList] public PlanetData[] planets;
}