using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] Cooldown cooldown;
    public float minCooldownDeviation = 0.1f;
    public float maxCooldownDeviation = 4f;
    public int minSpawnersTrigger = 4;
    public int maxSpawnersTrigger = 6;
    public float affectRadius = 10f;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (cooldown.IsCoolingDown) return;

        int count = Random.Range(minSpawnersTrigger, maxSpawnersTrigger);

        GetComponentsInChildren<EnemySpawner>().
            OrderBy(obj => (obj.transform.position - PlayerManager.instance.player.transform.position).sqrMagnitude).
            Take(count).
            ToList().
            ForEach(spawner =>
        {
            if ((spawner.transform.position - PlayerManager.instance.player.transform.position).magnitude > affectRadius) return;
            StartCoroutine(spawner.SpawnEnemyWithDelay(Random.Range(minCooldownDeviation, maxCooldownDeviation)));
        });

        cooldown.StartCooldown();
    }

    void OnDrawGizmosSelected()
    {
        if (PlayerManager.instance == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(PlayerManager.instance.player.transform.position, affectRadius);
    }
}
