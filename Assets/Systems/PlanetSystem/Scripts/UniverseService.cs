using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class UniverseService : MonoBehaviour, IUniverseService
{
    [SerializeField] private PlanetDatabase planetDatabase;

    private readonly List<Planet> _planets = new();

    private void Awake()
    {
        ServiceLocator.Instance.Register<IUniverseService>(this);
    }

    private void Start()
    {
        for (int i = 0; i < planetDatabase.planets.Length; ++i)
        {
            Planet planet = Instantiate(planetDatabase.planets[i].planet);
            planet.gameObject.SetActiveFast(false);
            _planets.Add(planet);
        }
    }
    
    public void SwitchPlanet(string id)
    {
        throw new System.NotImplementedException();
    }
}
