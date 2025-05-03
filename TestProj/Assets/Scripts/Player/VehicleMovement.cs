using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;



public class VehicleMovement : MonoBehaviour
{
    [SerializeField] private List<WheelCollider> leftWheelsColliders;
    [SerializeField] private List<WheelCollider> rightWheelsColliders;
    
    [Header("Vehicle lower body params")]
    public float motorForce;      
    public float brakeForce;     
    public float rotationSpeed; 
    public float maxAngularSpeed;
    public float maxSpeed;     

    [Header("Barrel params")]
    [SerializeField] private GameObject barelPrefab;

    // in reality max and min elevation reversed 
    [SerializeField] private float maxElevation; 
    [SerializeField] private float minElevation; 

    [SerializeField] private float barrelRotationSpeed; 
    [SerializeField] private float barrelSmoothTime; 
    
    //current input params for lower body
    private float verticalInput;         
    private float horizontalInput;        
    private float currBrakeForce;
    
    //current input params for barrel
    private float barrelInputY;
    private float currentBarrelVelocity; // For smoothing
    private float targetElevation;
    private float currentElevation;

    private float currSpeed = 0;
    [SerializeField] private Rigidbody rb;
    private PlayerInputControls input;
    
    void Start()
    {
        input = PlayerManager.instance.input;
        //rb = GetComponent<Rigidbody>();
        
        // Lower center of mass for stability
        rb.centerOfMass = new Vector3(0, -0.8f, 0);
        
        // Initialize barrel position
        if (barelPrefab != null)
        {
            Vector3 currentRotation = barelPrefab.transform.localEulerAngles;
            currentElevation = NormalizeAngle(currentRotation.x);
            targetElevation = currentElevation;
            
            // Ensure barrel starts at the correct initial position
            barelPrefab.transform.localEulerAngles = new Vector3(currentElevation, currentRotation.y, currentRotation.z);
        }
    }
    
    void FixedUpdate()
    {
        verticalInput = input.playerInput.Move.y;    // forward/reverse
        horizontalInput = input.playerInput.Move.x;  // turn right/left
        currBrakeForce = input.playerInput.Break ? brakeForce : 0f;
        
        // Get barrel input - using Y for vertical movement is more intuitive
        barrelInputY = input.playerInput.BarrelMoving.y;

        // Выводим значение для отладки
        // if (Mathf.Abs(barrelInputY) > 0.1f)
        // {
        //     Debug.Log($"Raw Barrel Input: {barrelInputY}, Raw Vector: {input.playerInput.BarrelMoving}");
        // }

        ApplyMotor();
        ElevateBarrel();
        
        // quaternion rotating
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            ApplyRotation();
        }
        else
        {  
            StopRotation();
        }
    }

    // Helper function to normalize angles to -180 to 180 range
    private float NormalizeAngle(float angle)
    {
        // Преобразуем угол в диапазон -180 до 180
        while (angle > 180f)
            angle -= 360f;
        while (angle < -180f)
            angle += 360f;
        return angle;
    }

    void ElevateBarrel()
    {
        if (barelPrefab == null) return;

        // get barrel local pos
        Vector3 currentRotation = barelPrefab.transform.localEulerAngles;
        float currentXangle = NormalizeAngle(currentRotation.x);
        
        // Update target elevation based on input (note: using Y for up/down movement)
        // Only move when there's input
        if (Mathf.Abs(barrelInputY) > 0.1f)
        {
            // check limits
            bool canMoveUp = (barrelInputY > 0 && currentXangle < maxElevation);
            bool canMoveDown = (barrelInputY < 0 && currentXangle > minElevation);
            
            if (canMoveUp || canMoveDown)
            {
                // calc new pos
                targetElevation += barrelInputY * barrelRotationSpeed * Time.deltaTime;
                targetElevation = Mathf.Clamp(targetElevation, minElevation, maxElevation);
                
                // move barrel to target pos
                currentElevation = Mathf.SmoothDamp(currentElevation, targetElevation, ref currentBarrelVelocity, barrelSmoothTime);
                
                // set new angle 
                barelPrefab.transform.localEulerAngles = new Vector3(currentElevation, currentRotation.y, currentRotation.z);
            }
            
            // Отладочный вывод
            //Debug.Log($"Barrel Input: {barrelInputY}, Target: {targetElevation}, Current: {currentElevation}, CurrentX: {currentXangle}, CanUp: {canMoveUp}, CanDown: {canMoveDown}");
        }
        // Когда нет ввода, оставляем ствол в текущем положении
    }

    void ApplyMotor()
    {
        // get force for all wheels
        float motorTorque = verticalInput * motorForce;

        Vector3 velocity = rb.velocity;
        currSpeed = velocity.magnitude;
        if(velocity.magnitude < maxSpeed)
        {
            foreach (var wheel in leftWheelsColliders)
            {
                wheel.motorTorque = motorTorque;
                wheel.brakeTorque = currBrakeForce;
            }
            
            foreach (var wheel in rightWheelsColliders)
            {
                wheel.motorTorque = motorTorque;
                wheel.brakeTorque = currBrakeForce;
            }
        }
        else
        {
            rb.velocity = velocity.normalized * maxSpeed;
        }
    }
    
    void ApplyRotation()
    {
        float rotationAmount = horizontalInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, rotationAmount, 0);
        rb.MoveRotation(rb.rotation * turnRotation);

        // rotate on the point
        if (Mathf.Abs(verticalInput) < 0.1f)
        {
            // max rotation speed check
            if (Mathf.Abs(rb.angularVelocity.y) < maxAngularSpeed)
            {
                rb.AddTorque(transform.up * horizontalInput * rotationSpeed * 0.8f, ForceMode.Acceleration);
            }

            // kill side rotation
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            rb.AddForce(-transform.right * localVelocity.x * 2.0f, ForceMode.Acceleration);
        }
    }
    
    void StopRotation()
    {
        // Stop the angular velocity when the turn button is not pressed
        Vector3 angularVelocity = rb.angularVelocity;
        
        // Decrease the angular velocity along the Y axis (rotation in the horizontal plane)
        if (Mathf.Abs(angularVelocity.y) > 0.01f)
        {
            // We stop the rotation smoothly
            angularVelocity.y *= 0.9f;
            rb.angularVelocity = angularVelocity;
        }
        else
        {
            // Stop rotation along the Y axis completely when it is already slow
            angularVelocity.y = 0;
            rb.angularVelocity = angularVelocity;
        }
    }
}


// public class VehicleMovement : MonoBehaviour
// {
//     // Вместо разделения на передние и задние, разделим на левые и правые
//     [SerializeField] private List<WheelCollider> leftWheelsColliders;
//     [SerializeField] private List<WheelCollider> rightWheelsColliders;
    
//     public float motorForce = 1500f;
//     public float brakeForce = 3000f;
//     public float turnIntensity = 0.9f; // Коэффициент поворота (0.5 = 50% от полной мощности)
    
//     private float verticalInput; // Вперед/назад
//     private float horizontalInput; // Поворот влево/вправо
//     private float currBrakeForce;
//     private PlayerInputControls input;
    
//     void Start()
//     {
//         input = PlayerManager.instance.input;
        
//         // Понижаем центр масс для большей стабильности
//         GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -0.5f, 0);
//     }
    
//     void FixedUpdate()
//     {
//         verticalInput = input.playerInput.Move.y; // Вперед/назад
//         horizontalInput = input.playerInput.Move.x; // Поворот влево/вправо
//         currBrakeForce = input.playerInput.Break ? brakeForce : 0f;
        
//         // Применяем танковое управление
//         ApplyTankSteering();
//     }
    
//     void ApplyTankSteering()
//     {
//         float leftMotorPower = 0;
//         float rightMotorPower = 0;
        
//         // Рассчитываем мощность для левой и правой стороны
//         if (verticalInput != 0)
//         {
//             // При движении вперед или назад
//             leftMotorPower = verticalInput * motorForce * (1 - horizontalInput * turnIntensity);
//             rightMotorPower = verticalInput * motorForce * (1 + horizontalInput * turnIntensity);
//         }
//         else if (horizontalInput != 0)
//         {
//             // Поворот на месте
//             leftMotorPower = -horizontalInput * motorForce * turnIntensity;
//             rightMotorPower = horizontalInput * motorForce * turnIntensity;
//         }
        
//         // Применяем мощность к левым колесам
//         foreach (var wheel in leftWheelsColliders)
//         {
//             wheel.motorTorque = leftMotorPower;
//             wheel.brakeTorque = currBrakeForce;
//             wheel.steerAngle = 0; // Убираем поворот колес
//         }
        
//         // Применяем мощность к правым колесам
//         foreach (var wheel in rightWheelsColliders)
//         {
//             wheel.motorTorque = rightMotorPower;
//             wheel.brakeTorque = currBrakeForce;
//             wheel.steerAngle = 0; // Убираем поворот колес
//         }
//     }
    
//     // Если нужно визуально обновлять колеса
//     void UpdateWheelVisuals()
//     {
//         // Здесь код для обновления визуальных моделей колес
//     }
// }