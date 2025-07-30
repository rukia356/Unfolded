using Unity.Netcode;
using UnityEngine;

public class ButtonScript : MonoBehaviour 
{
    public bool isPressed = false;

    [SerializeField] private PuzzleManager puzzleManager;

    private void OnTriggerEnter(Collider other)
    {
        //if (!NetworkManager.Singleton.IsServer) return;
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Button pressed by: " + other.name);
            isPressed = true;
            Debug.Log(gameObject.name + " pressed button");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Button not pressed by: " + other.name);
            isPressed = false;
            Debug.Log(gameObject.name + " released button");
        }
    }

}
