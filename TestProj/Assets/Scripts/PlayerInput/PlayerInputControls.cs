using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInput
{
    // public Quaternion Rotation;
    public Vector2 Look;
    public Vector2 Move;
    public bool Break;

    public bool ShootHold;
    public bool ShootPress;

    public Vector2 BarrelMoving;

}

public class PlayerInputControls : MonoBehaviour
{
    public PlayerInput playerInput { get; private set; }
    PlayerControls controls;

    

    void OnDestroy()
    {
        controls.Dispose();
    }

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        playerInput = new PlayerInput
        {
            // Rotation = playerCamera.transform.rotation,
            Look = controls.Player.Look.ReadValue<Vector2>(),
            Move = controls.Player.Move.ReadValue<Vector2>(),
            Break = controls.Player.Jump.ReadValue<float>() > 0.5f,

            ShootHold = controls.Player.Fire.ReadValue<float>() > 0.5f, 
            ShootPress = controls.Player.Fire.WasPressedThisFrame(),
            
            //barrel moving   
            BarrelMoving = controls.Player.BarelMove.ReadValue<Vector2>()

        };
    }
}
