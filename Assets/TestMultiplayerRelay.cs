using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
//using Unity.Services.Relay;
//using Unity.Services.Multiplayer;
using UnityEngine;
//using Unity.Services.Multiplay; // new version of relay ish
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
//using Unity.Services.Lobbies;
//using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models; // this part stays for join code/allocations
using Unity.Services.Relay;
using Unity.Networking.Transport.Relay;

public class TestMultiplayerRelay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            //await AuthenticationService.Instance.SignInWithEmailAsync(email, password);
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        }

        await CreateRelayAndJoin();
        //AuthenticationService.Instance.SignedIn += () =>
        //{
        //    Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        //};

        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async System.Threading.Tasks.Task CreateRelayAndJoin()
    {
        try
        {
            // made relay allocation
            var allocation = await Relay.Instance.CreateAllocationAsync(2);

            // getting join code
            string joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Join code: " + joinCode);

            // setting up transport through relay data
            var relayServerData = new RelayServerData(allocation, "dtls");

            // assigning unity transport
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(relayServerData);

            // start hosting
            NetworkManager.Singleton.StartHost();
            Debug.Log("Hosting started with Relay.");
        }
        catch (Exception e)
        {
            Debug.LogError("Relay setup has failed: " + e.Message);
        }
    }

    //private async void CreateRelay()
    //{
    //    try
    //    {
    //        await RelayService.Instance.CreateAllocationAsync(2);
    //    } catch (RelayServiceException e)
    //    {
    //        Debug.Log(e);
    //    }
    //}

}
