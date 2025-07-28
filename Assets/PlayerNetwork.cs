using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class PlayerNetwork : NetworkBehaviour
{

    public float speed = 5f;
    public CharacterController controller;
    //public Transform cam;
    public Camera cam;


    [SerializeField] private Transform spawnedObjectPrefab;
    
    private Transform spawnedObjectTransform;

    private Camera playerCamera;

    private NetworkVariable<CustomData> randomNumber = new NetworkVariable<CustomData>(
        new CustomData
        {
            _int = 56,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct CustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (CustomData previousValue, CustomData newValue) =>
        {
            Debug.Log(OwnerClientId + ";  " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };

        if (IsOwner)
        {
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                playerCamera.enabled = true;
            }
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.T))
        {
            // TestServerRpc("Message here" or new ServerRpcParmams);  comment out anything else inside and uncomment this if want to use rpc
            // TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List <ulong> { 1 } } } );
            spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

            //randomNumber.Value = new CustomData
            //{
            //    _int = 10,
            //    _bool = false,
            //    message = "Did you see a screwdriver anywhere?"
            //};
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(spawnedObjectTransform.gameObject);
            // option for despawning
            // spawnedObjectTransform.GetComponent<NetworkObject>().Despawn(true);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10f;  
        }
        else
        {
            speed = 5f;   
        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        //if (Input.GetKey(KeyCode.Space)) moveDir.y = +1f;

        //float moveSpeed = 3f;
        //transform.position += moveDir * moveSpeed * Time.deltaTime;
        transform.position += moveDir * speed * Time.deltaTime;

    }

    [ServerRpc]               //ServerRpcParams serverRpcParams
    private void TestServerRpc(string message)
    {
        Debug.Log("TestServerRpc " + OwnerClientId + "; " + message);
        // Debug.Log("TestServerRpc " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("TestClientRpc");
    }

}

//private float moveSpeed = 3f;
//private float sprintSpeed = 6f;
//private float crouchSpeed = 1.5f;
//private float jumpForce = 5f;
//private float gravity = -9.81f;
//private float verticalVelocity = 0f;
//private bool isGrounded = false;

//[SerializeField] private Transform groundCheck;
//[SerializeField] private float groundCheckDistance = 0.2f;
//[SerializeField] private LayerMask groundMask;

//private void Update()
//{
//    if (!IsOwner) return;

//    HandleMovement();
//}

//private void HandleMovement()
//{
//    // --- Ground Check ---
//    isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

//    if (isGrounded && verticalVelocity < 0)
//        verticalVelocity = -2f; // Keeps grounded

//    // --- Input ---
//    Vector3 moveDir = Vector3.zero;

//    if (Input.GetKey(KeyCode.W)) moveDir += transform.forward;
//    if (Input.GetKey(KeyCode.S)) moveDir -= transform.forward;
//    if (Input.GetKey(KeyCode.A)) moveDir -= transform.right;
//    if (Input.GetKey(KeyCode.D)) moveDir += transform.right;

//    moveDir.Normalize();

//    float currentSpeed = moveSpeed;
//    if (Input.GetKey(KeyCode.LeftShift)) currentSpeed = sprintSpeed;
//    else if (Input.GetKey(KeyCode.LeftControl)) currentSpeed = crouchSpeed;

//    // --- Jump ---
//    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);

//    // --- Apply Gravity ---
//    verticalVelocity += gravity * Time.deltaTime;

//    // Final move vector
//    Vector3 finalMove = moveDir * currentSpeed;
//    finalMove.y = verticalVelocity;

//    transform.position += finalMove * Time.deltaTime;
//}
