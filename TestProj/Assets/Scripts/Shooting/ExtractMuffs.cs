using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractMuffs : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;  
    [SerializeField] private Transform spawnPoint;     
    [SerializeField] private float launchForce = 1f; 
    [SerializeField] private float torqueForce = 1f; 

    
    public void SpawnAndLaunch()
    {
        GameObject spawnedObject = Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            float angleUp = 20f;
            float angleRight = 30f;

            Vector3 direction = Quaternion.AngleAxis(-angleUp, spawnPoint.right) * spawnPoint.forward;
            direction = Quaternion.AngleAxis(angleRight, Vector3.up) * direction;

            rb.AddForce(direction.normalized * launchForce, ForceMode.Impulse);

            rb.AddTorque(spawnPoint.forward * Random.Range(1f, torqueForce), ForceMode.Impulse);
        }

        Destroy(spawnedObject, 10f);
    }

}
