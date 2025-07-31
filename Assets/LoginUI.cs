using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private GameObject multiplayerUI; // Panel with Host/Join buttons
    [SerializeField] private GameObject loginPanel;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        loginButton.onClick.AddListener(async () => await Login());
        registerButton.onClick.AddListener(async () => await Register());
    }

    private async Task Login()
    {
        try
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                AuthenticationService.Instance.SignOut();
            }

            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(
            usernameField.text, passwordField.text);
            Debug.Log("Login successful!");

            multiplayerUI.SetActive(true);  // show multiplayer menu
            //gameObject.SetActive(false);    // hide login panel
            loginPanel.SetActive(false);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError("Login failed: " + ex.Message);
        }
    }

    private async Task Register()
    {
        try
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                AuthenticationService.Instance.SignOut();
            }

            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(
            usernameField.text, passwordField.text);
            Debug.Log("Registration successful! You can now log in.");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError("Registration failed: " + ex.Message);
        }
    }
}