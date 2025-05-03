using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    float maxHealth { get; set; }
    float currentHealth { get; set; }
    

    void Damage(float damageAmount);
    void Die();
}
