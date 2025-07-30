using UnityEngine;
using Unity.Netcode;

public class KeyPickup : NetworkBehaviour
{
    // test code to see if working with just collider
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!IsServer) return;

    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log($"{other.name} picked up the key!");

    //        // Tell the player they have the key
    //        PlayerNetwork player = other.GetComponent<PlayerNetwork>();
    //        if (player != null)
    //        {
    //            player.GiveKeyServerRpc();
    //        }

    //        // Despawn the key on the network
    //        GetComponent<NetworkObject>().Despawn();
    //    }
    //}

    private bool isInTrigger = false;
    private GameObject localPlayer;

    private void Update()
    {
        if (!IsOwner) return;

        if (isInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory inventory = localPlayer.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.HasKey.Value = true;
                if (IsServer)
                {
                    NetworkObject.Despawn(true);
                }
                else
                {
                    RequestDespawnServerRpc();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<NetworkObject>().IsOwner)
        {
            isInTrigger = true;
            localPlayer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<NetworkObject>().IsOwner)
        {
            isInTrigger = false;
            localPlayer = null;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestDespawnServerRpc()
    {
        NetworkObject.Despawn(true);
    }
}