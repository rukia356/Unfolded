using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Services.Authentication;
using Unity.Collections;


public class PlayerNetworkData : NetworkBehaviour
{
    [SerializeField] private TMP_Text usernameText;

    [SerializeField] private Renderer capsuleRenderer;


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

        playerColor.OnValueChanged += (oldColor, newColor) =>
        {
            capsuleRenderer.material.color = newColor;
        };

        capsuleRenderer.material.color = playerColor.Value;
    }

    public void SetPlayerColor(Color color)
    {
        if (IsOwner)
        {
            SubmitColorServerRpc(color);
        }
    }

    private NetworkVariable<Color> playerColor = new NetworkVariable<Color>();

    [ServerRpc]
    private void SubmitColorServerRpc(Color color)
    {
        playerColor.Value = color;
    }

    [ServerRpc]
    private void SubmitUsernameServerRpc(string name)
    {
        username.Value = name;
    }


}
