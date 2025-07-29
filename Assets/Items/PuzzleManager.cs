using Unity.Netcode;
using UnityEngine;

public class PuzzleManager : NetworkBehaviour
{
    public ButtonScript button1;
    public ButtonScript button2;

    public GameObject keyPrefab;
    public Transform keySpawnPoint;

    private bool keySpawned = false;

    private void Update()
    {
        if (!IsServer) return; // only host handling

        if (!keySpawned && button1.isPressed && button2.isPressed)
        {
            SpawnKeyServerRpc();
            keySpawned = true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKeyServerRpc()
    {
        Debug.Log("SpawnKeyServerRpc called!");
        GameObject key = Instantiate(keyPrefab, keySpawnPoint.position, Quaternion.identity);
        //Transform key = Instantiate(keyPrefab, keySpawnPoint.position, Quaternion.identity);
        key.GetComponent<NetworkObject>().Spawn();
    }

    public void CheckButtons()
    {
        Debug.Log("Checking buttons: " + button1.isPressed + ", " + button2.isPressed);
        if (button1.isPressed && button2.isPressed)
        {
            Debug.Log("Both buttons pressed — trying to spawn key");
            SpawnKeyServerRpc();
        }
    }
}
