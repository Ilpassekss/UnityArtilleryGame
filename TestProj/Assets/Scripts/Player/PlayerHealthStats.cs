using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthStats : HealthStats
{

    public GameObject deathcanvas;
    public override void Die()
    {
        base.Die();

        deathcanvas.SetActive(true);

        // Destroy(gameObject);
    }
}
