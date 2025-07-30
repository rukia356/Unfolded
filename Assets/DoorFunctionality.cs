using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement; // in case if scene management route

public class DoorFunctionality : MonoBehaviour
{

    // test code with just collider instead of button press
    //private void OnTriggerEnter(Collider other)
    //{
    //    //if (!IsServer) return;
    //    if (!Unity.Netcode.NetworkManager.Singleton.IsServer) return;

    //    if (other.CompareTag("Player"))
    //    {
    //        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
    //        if (inventory != null && inventory.HasKey.Value)
    //        {
    //            ShowWinScreenClientRpc();
    //        }
    //    }
    //}

    private bool isInTrigger = false;
    private GameObject localPlayer;

    private void Update()
    {
        // Only run this on the local player
        if (isInTrigger && localPlayer != null && localPlayer.GetComponent<NetworkObject>().IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerInventory inventory = localPlayer.GetComponent<PlayerInventory>();
                if (inventory != null && inventory.HasKey.Value)
                {
                    ShowWinScreenClientRpc();
                }
                else
                {
                    Debug.Log("You need the key to open this door.");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NetworkObject netObj = other.GetComponent<NetworkObject>();
            if (netObj != null && netObj.IsOwner)
            {
                isInTrigger = true;
                localPlayer = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NetworkObject netObj = other.GetComponent<NetworkObject>();
            if (netObj != null && netObj.IsOwner)
            {
                isInTrigger = false;
                localPlayer = null;
            }
        }
    }



    [ClientRpc]
    private void ShowWinScreenClientRpc(ClientRpcParams rpcParams = default)
    {
        NetworkManagerUI ui = FindAnyObjectByType<NetworkManagerUI>();
        if (ui != null)
        {
            ui.ShowWinScreen();
        }
    }
}
