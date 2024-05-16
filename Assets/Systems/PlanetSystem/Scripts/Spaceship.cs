using System;
using System.Collections;
using System.Collections.Generic;
using SuperMaxim.Messaging;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private PlanetDatabase planetDatabase;
    private void Awake()
    {
        Messenger.Default.Subscribe<CodePayload>(OnCodeReceived);
    }

    //TODO fill in functionality
    private void OnCodeReceived(CodePayload codePayload)
    {
        Debug.Log($"Go to a planet with code {codePayload.code}!");
        // check whether there is a valid planet according to the code
        // get the planet id from the database, the uncomment the following line
        string id = planetDatabase.GetPlanet(codePayload.code);
        ServiceLocator.Instance.Get<IUniverseService>().SwitchPlanet(id);
    }
}
