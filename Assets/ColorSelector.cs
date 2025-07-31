using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;


public class ColorSelector : MonoBehaviour
{
    public static ColorSelector Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectColor(Color color)
    {
        var player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerNetworkData>();
        player.SetPlayerColor(color);
        SaveColorToCloud(color);
    }

    public void OnGreenButtonClick()
    {
        ColorSelector.Instance.SelectColor(Color.green);
    }

    public void OnBlueButtonClick()
    {
        ColorSelector.Instance.SelectColor(Color.blue);
    }

    public void OnYellowButtonClick()
    {
        ColorSelector.Instance.SelectColor(Color.yellow);
    }

    public void OnBlackButtonClick()
    {
        ColorSelector.Instance.SelectColor(Color.black);
    }

    private async void SaveColorToCloud(Color color)
    {
        string colorHex = ColorUtility.ToHtmlStringRGB(color);

        var data = new Dictionary<string, object>
        {
            { "playerColor", colorHex }
        };

        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        Debug.Log("Saved color: " + colorHex);
    }
}
