using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ExplodeOnContact : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect; 

    [SerializeField] private AudioClip ExplosionSound;

    [SerializeField] private float explosionForce = 1000f;
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float upwardsModifier = 1.0f;

    [SerializeField] private float maxDamage = 100f;
    [SerializeField] private float minDamage = 20f;

    private Rigidbody rb;
    

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
         if (rb.velocity.sqrMagnitude > 0.1f)
        {
            // rotate obj
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void Explode()
    {
        
        // if (explosionEffect != null)
        // {
        //     Instantiate(explosionEffect, transform.position, Quaternion.identity);
        // }

        if(ExplosionSound != null)
        {
            AudioManager.AudioManagerInstance.PlaySound(ExplosionSound);
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        Debug.Log(colliders.Count());
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
            }

            if(hit.isTrigger)
            {
                HealthStats targetHealth = hit.transform.GetComponent<EnemyHealthStats>();

                Debug.Log(targetHealth);

                    if(targetHealth != null)
                    {
                        float distance = Vector3.Distance(transform.position, hit.transform.position);
                        int damage = (int)Mathf.Lerp(maxDamage, minDamage, distance / explosionRadius);
                        targetHealth.TakeDamage(damage);
                    }  
            }

            
        }
        
        Destroy(gameObject);
    }
}

