using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cooldown
{
    [SerializeField] float cooldownTime = 2f;
    float nextFireTime;

    public bool IsCoolingDown => Time.time < nextFireTime;
    public void StartCooldown() => nextFireTime = Time.time + cooldownTime;
}
