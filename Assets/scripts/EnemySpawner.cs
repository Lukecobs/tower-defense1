using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPoint;  // Spawn location
    public Transform[] waypoints; // Waypoints for enemy movement

    // Method to spawn a specific enemy prefab
    public void SpawnEnemy(GameObject enemyPrefab)
    {
        // Instantiate the enemy at the spawn point
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Initialize the enemy with the waypoints
        newEnemy.GetComponent<EnemyBase>().Initialize(waypoints);
    }
}
