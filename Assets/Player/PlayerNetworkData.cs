using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Services.Authentication;
using Unity.Collections;

public class PlayerNetworkData : NetworkBehaviour
{
    [SerializeField] private TMP_Text usernameText;

    //private NetworkVariable<string> username = new NetworkVariable<string>();
    private NetworkVariable<FixedString64Bytes> username = new NetworkVariable<FixedString64Bytes>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Set your own username from Authentication
            string currentUsername = AuthenticationService.Instance.PlayerName;
            SubmitUsernameServerRpc(currentUsername);
        }

        username.OnValueChanged += (oldValue, newValue) =>
        {
            usernameText.text = username.Value.ToString();
        };

        // Set immediately if already synced
        usernameText.text = username.Value.ToString();
    }

    [ServerRpc]
    private void SubmitUsernameServerRpc(string name)
    {
        username.Value = name;
    }
}
