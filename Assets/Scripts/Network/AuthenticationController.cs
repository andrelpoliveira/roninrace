using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;

public class AuthenticationController : MonoBehaviour
{
    [Header("Variáveis de Controle")]
    public bool eventsInitialized = false;

    public void StartGame()
    {
        StartClientService();
    }

    public async void StartClientService()
    {
        try
        {
            if(UnityServices.State != ServicesInitializationState.Initialized)
            {
                var options = new InitializationOptions();
                options.SetProfile("default_profile");
                await UnityServices.InitializeAsync();
                Debug.Log(UnityServices.State);
            }

            if (!eventsInitialized) 
            {
                SetupEvents();
            }

            if (AuthenticationService.Instance.SessionTokenExists)
            {

            }
            else
            {

            }

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    void SetupEvents()
    {
        eventsInitialized = true;
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }
}
