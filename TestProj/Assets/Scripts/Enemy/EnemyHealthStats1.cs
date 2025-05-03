using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthStats : HealthStats
{
    public override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
