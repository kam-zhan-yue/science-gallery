using System.Collections.Generic;
using DG.Tweening;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class UniverseService : MonoBehaviour, IUniverseService
{
    [SerializeField] private PlanetDatabase planetDatabase;

    private readonly Dictionary<string,Planet> _planets = new();

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
            _planets.Add(planetDatabase.planets[i].id, planet);
        }
    }
    
    public void SwitchPlanet(string id)
    {
        Debug.Log(id);
        // Loop through _planets to find the id that should be on and then turn it on
        _planets[id].gameObject.SetActiveFast(true);
        Sequence sequence = DOTween.Sequence();
        
        // Create a fade out tween
        Tween fadeOut = _planets[id].SpriteRenderer.DOFade(0f, 1f);
        // Append it to the sequence
        sequence.Append(fadeOut);
        
        // Create a fade in tween
        Tween fadeIn = _planets[id].SpriteRenderer.DOFade(0f, 1f);
        // Append it to the sequence
        sequence.Append(fadeIn);
        
        
        
        sequence.OnComplete(() =>
        {
            PlanetDatabase.PlanetData data = planetDatabase.GetPlanetDataById(id);
            ServiceLocator.Instance.Get<IDialogueService>().Play(data?.dialogueId);
        });
        sequence.Play();
    }
}
