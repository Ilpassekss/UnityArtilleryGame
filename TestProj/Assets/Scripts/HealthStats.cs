using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        
        damage = Math.Max(0, damage);

        currentHealth -= damage;
        Debug.Log($"{transform.name}: damage - {damage}, remaining life - {currentHealth}");
        if(currentHealth <= 0)
        {
            Die();
        }
    } 

    public virtual void Die()
    {
        // must be overwritten depending on desired implementation;
        Debug.Log($"{transform.name} died");
    } 
}
