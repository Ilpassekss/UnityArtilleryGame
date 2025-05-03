using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public struct CameraInput
{
    public Vector2 Look;
}
// public class PlayerCamera : MonoBehaviour
// {
//     public Quaternion quatRotation => playerCamera.transform.rotation;
//     public float mouseSensitivity = 0.5f;
//     public float topClamp = 89.0f;
//     public float bottomClamp = -89.0f;

//     PlayerInputControls input;
//     Camera playerCamera;

//     [SerializeField] private Transform targetObj;
//     [SerializeField] public Vector3 cameraOffset; // distance between 

//     Vector3 rotation;
//     void Start()
//     {
//         cameraOffset = transform.position - targetObj.transform.position;

//         Cursor.lockState = CursorLockMode.Locked;
//         input = PlayerManager.instance.input;
//         playerCamera = GetComponentInChildren<Camera>();
//     }

//     void LateUpdate()
//     {
//         Vector2 inputVector = input.playerInput.Look;
//         // rotation angles 
//         rotation.y += inputVector.x * mouseSensitivity;
//         rotation.x -= inputVector.y * mouseSensitivity;
//         rotation.x = Mathf.Clamp(rotation.x, bottomClamp, topClamp);
//         //rotating
//         Quaternion cameraRotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);

//         //set position 
//         Vector3 offsetRoated = cameraRotation * cameraOffset;
//         //update position
//         transform.position = targetObj.position + offsetRoated;
//         playerCamera.transform.rotation = cameraRotation;

//         // playerCamera.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);


//         // Vector3 newPosition  = targetObj.transform.position + cameraOffset;
//         // transform.position = newPosition;

    
//     }
// }



public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity;
    public float topClamp;
    public float bottomClamp;
    public float radius = 5.0f;

    private Vector2 rotation = Vector2.zero;

    private PlayerInputControls input;

    [SerializeField] private Transform targetObj;

    [SerializeField] private GameObject upperBody;
    [SerializeField] private GameObject lowerBody;

    [SerializeField] private float rotationSpeed;

   
    private Camera playerCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        input = PlayerManager.instance.input;
        playerCamera = GetComponentInChildren<Camera>();
    }

    void LateUpdate()
    {   
        
        Vector2 inputVector = input.playerInput.Look;

        rotation.x += inputVector.x * mouseSensitivity;
        rotation.y -= inputVector.y * mouseSensitivity;
        rotation.y = Mathf.Clamp(rotation.y, bottomClamp, topClamp);

        // 
        Quaternion rotationQuat = Quaternion.Euler(rotation.y, rotation.x, 0);
        Vector3 offset = rotationQuat * new Vector3(0, 0, -radius);

        
        playerCamera.transform.position = targetObj.position + offset;
        playerCamera.transform.LookAt(targetObj);

        //RotateUpperBody();
    }


    void Update()
    {
        RotateUpperBody();
    }

    // Rotate the upper body based on the camera's horizontal rotation
    void RotateUpperBody()
    {
        if (upperBody != null && lowerBody != null)
        {
            // Get the camera's rotation on the Y-axis (horizontal plane)
            float horizontalRotation = playerCamera.transform.eulerAngles.y;
            
            // Set the local rotation of the upper body - this preserves its position
            upperBody.transform.localRotation = Quaternion.Slerp(
                upperBody.transform.localRotation, 
                Quaternion.Euler(0f, horizontalRotation - lowerBody.transform.eulerAngles.y, 0f), 
                Time.deltaTime * rotationSpeed
            );
        }
    }
}

