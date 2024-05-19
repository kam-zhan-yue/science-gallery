using System.Collections.Generic;
using DG.Tweening;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class UniverseService : MonoBehaviour, IUniverseService
{
    [SerializeField] private PlanetDatabase planetDatabase;

    private readonly Dictionary<string,Planet> _planets = new();

    private string _active = string.Empty;

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
        Debug.Log($"Fade out {_active}");
        Debug.Log($"Fade in {id}");
        // Loop through _planets to find the id that should be on and then turn it on
        Sequence sequence = DOTween.Sequence();

        // If there is an active planet, fade it out
        if (_planets.TryGetValue(_active, out Planet activePlanet))
        {
            // Create a fade out tween
            Tween fadeOut = activePlanet.SpriteRenderer.DOFade(0f, 1f).OnComplete(() =>
            {
                activePlanet.gameObject.SetActiveFast(false);
            });
            // Append it to the sequence
            sequence.Append(fadeOut);
        }

        if (_planets.TryGetValue(id, out Planet nextPlanet))
        {
            nextPlanet.gameObject.SetActiveFast(true);
            Color color = nextPlanet.SpriteRenderer.color;
            color.a = 0f;
            nextPlanet.SpriteRenderer.color = color;
            // Create a fade in tween
            Tween fadeIn = nextPlanet.SpriteRenderer.DOFade(1f, 1f);
            // Append it to the sequence
            sequence.Append(fadeIn);
        }

        _active = id;

        sequence.OnComplete(() =>
        {
            PlanetDatabase.PlanetData data = planetDatabase.GetPlanetDataById(id);
            ServiceLocator.Instance.Get<IDialogueService>().Play(data?.dialogueId);
        });
        sequence.Play();
    }
}
