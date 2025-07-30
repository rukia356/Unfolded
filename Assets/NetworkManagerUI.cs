using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System.Threading.Tasks;
using Unity.Networking.Transport.Relay;
using System.Runtime.CompilerServices;
using UtpRelay = Unity.Services.Relay.Models;
using Relay = Unity.Services.Relay;
using RelayModels = Unity.Services.Relay.Models;

// handles UI buttons for server, host and client

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button ServerBtn;
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button ClientBtn;

    [Header("Win Screen")]
    [SerializeField] private GameObject winScreenPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Relay UI")]
    [SerializeField] private Button HostGameBtn;
    [SerializeField] private Button JoinGameBtn;
    [SerializeField] private TMP_InputField JoinCodeInputField;
    [SerializeField] private TMP_Text joinCodeDisplay;



    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        restartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        ServerBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });

        HostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        ClientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });

        HostGameBtn.onClick.AddListener(async () =>
        {
            await CreateRelay();
        });

        JoinGameBtn.onClick.AddListener(async () =>
        {
            string joinCode = JoinCodeInputField.text;
            await JoinRelay(joinCode);
        });
    }

    private async Task CreateRelay()
    {
        try
        {
            Unity.Services.Relay.Models.Allocation allocation =
                await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(2); // 2 clients

            string joinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Join Code: " + joinCode);

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(new RelayServerData(allocation, "dtls"));

            joinCodeDisplay.text = "Join Code: " + joinCode;

            NetworkManager.Singleton.StartHost();
        }
        catch (Unity.Services.Relay.RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private async Task JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining with code: " + joinCode);

            Unity.Services.Relay.Models.JoinAllocation allocation =
                await Unity.Services.Relay.RelayService.Instance.JoinAllocationAsync(joinCode);

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(new RelayServerData(allocation, "dtls"));

            NetworkManager.Singleton.StartClient();
        }
        catch (Unity.Services.Relay.RelayServiceException e)
        {
            Debug.LogError(e);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowWinScreen()
    {
        winScreenPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}

//public class NetworkManagerUI : MonoBehaviour
//{
//    [SerializeField] private Button ServerBtn;
//    [SerializeField] private Button HostBtn;
//    [SerializeField] private Button ClientBtn;

//    [SerializeField] private Button HostGameBtn;
//    [SerializeField] private Button JoinGameBtn;
//    [SerializeField] private TMP_InputField JoinCodeInputField;

//    // prior tests, currently not active, ignore.
//    //[Header("Relay UI")]
//    //[SerializeField] private Button CreateGameBtn;
//    //[SerializeField] private TMP_InputField JoinCodeInputField;
//    //[SerializeField] private Button JoinGameBtn;

//    private async void Awake()
//    {
//        await UnityServices.InitializeAsync();
//        await AuthenticationService.Instance.SignInAnonymouslyAsync();

//        ServerBtn.onClick.AddListener(() =>
//        {
//            NetworkManager.Singleton.StartServer();
//        });

//        HostBtn.onClick.AddListener(() =>
//        {
//            NetworkManager.Singleton.StartHost();
//        });

//        ClientBtn.onClick.AddListener(() =>
//        {
//            NetworkManager.Singleton.StartClient();
//        });

//        HostGameBtn.onClick.AddListener(async () => {
//            await CreateRelay();
//        });

//        JoinGameBtn.onClick.AddListener(async () => {
//            string joinCode = JoinCodeInputField.text;
//            await JoinRelay(joinCode);
//        });

//        //CreateGameBtn.onClick.AddListener(async () =>
//        //{
//        //    await CreateRelay();
//        //});

//        //JoinGameBtn.onClick.AddListener(async () =>
//        //{
//        //    string joinCode = JoinCodeInputField.text;
//        //    await JoinRelay(joinCode);
//        //});
//    }


//}


//// handles UI buttons for server, host and client
//public class NetworkManagerUI : MonoBehaviour
//{

//    [SerializeField] private Button ServerBtn;
//    [SerializeField] private Button HostBtn;
//    [SerializeField] private Button ClientBtn;

//    [Header("Relay UI")]
//    [SerializeField] private Button CreateGameBtn;
//    [SerializeField] private TMP_InputField JoinCodeInputField;
//    [SerializeField] private Button JoinGameBtn;

//    private async void Awake()
//    {
//        await UnityServices.InitializeAsync();
//        await AuthenticationService.Instance.SignInAnonymouslyAsync();

//        ServerBtn.onClick.AddListener(() =>
//        {
//            NetworkManager.Singleton.StartServer();
//        });

//        HostBtn.onClick.AddListener(() =>
//        {
//            NetworkManager.Singleton.StartHost();
//        });

//        ClientBtn.onClick.AddListener(() =>
//        {
//            NetworkManager.Singleton.StartClient();
//        });

//        CreateGameBtn.onClick.AddListener(async () =>
//        {
//            await CreateRelay();
//        });

//        CreateGameBtn.onClick.AddListener(async () =>
//        {
//            string joinCode = JoinCodeInputField.text;
//            await JoinRelay(joinCode);
//        });
//    }

//    private async Task CreateRelay()
//    {
//        try
//        {
//            Unity.Services.Relay.Models.Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
//            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

//            Debug.Log("Join Code: " + joinCode);

//            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
//            transport.SetRelayServerData(new RelayServerData(allocation, "dtls"));

//            NetworkManager.Singleton.StartHost();
//        }
//        catch (RelayServiceException e)
//        {
//            Debug.LogError(e);
//        }

//        private async Task JoinRelay(string joinCode)
//        {
//            try
//            {
//                Debug.Log("Joining Relay with " + joinCode);
//                Unity.Services.Relay.Models.JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

//                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
//                transport.SetRelayServerData(new RelayServerData(allocation, "dtls"));

//                NetworkManager.Singleton.StartClient();
//            }
//            catch (Unity.Services.Relay.RelayServiceException e)
//            {
//                Debug.LogError(e);
//            }
//        }
//    }
//}
//private async Task CreateRelay()
//{
//    try
//    {
//        UtpRelay.Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
//        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

//        Debug.Log("Join Code: " + joinCode);

//        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
//        transport.SetRelayServerData(new RelayServerData(allocation, "dtls"));

//        NetworkManager.Singleton.StartHost();
//    }
//    catch (Unity.Services.Relay.RelayServiceException e)
//    {
//        Debug.LogError(e);
//    }
//}

//private async Task JoinRelay(string joinCode)
//{
//    try
//    {
//        Debug.Log("Joining Relay with " + joinCode);
//        UtpRelay.JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

//        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
//        transport.SetRelayServerData(new RelayServerData(allocation, "dtls"));

//        NetworkManager.Singleton.StartClient();
//    }
//    catch (Unity.Services.Relay.RelayServiceException e)
//    {
//        Debug.LogError(e);
//    }
//}