using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float mouseSensitivity = 100f;

    public Transform playerBody;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    }
}
