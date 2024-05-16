using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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

    public bool ValidCode(string code)
    {
        for (int i = 0; i < planets.Length; ++i)
        {
            if (planets[i].code == code)
                return true;
        }
        return false;
    }
    public string GetPlanet(string code)
    {
        for (int i = 0; i < planets.Length; ++i)
        {
            if (planets[i].code == code)
                return planets[i].id;
        }
        return string.Empty;
    }
}