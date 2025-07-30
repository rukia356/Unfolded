using Unity.Netcode;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{
    public NetworkVariable<bool> HasKey = new NetworkVariable<bool>(false);
}
