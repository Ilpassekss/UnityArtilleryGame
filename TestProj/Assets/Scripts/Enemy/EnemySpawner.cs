using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    void Start()
    {
    }

    public IEnumerator SpawnEnemyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        var enemy = enemies[Random.Range(0, enemies.Length)];
        Instantiate(enemy, transform);
    }
    
    void FixedUpdate()
    {
        // if (cooldown.IsCoolingDown || enemies.Length == 0) return;

        
        

        // cooldown.StartCooldown();
    }
}
