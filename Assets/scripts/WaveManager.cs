using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;    // Reference to the EnemySpawner
    public float timeBetweenWaves = 5f;  // Time between waves
    private float waveCountdown;         // Countdown for next wave

    public Wave[] waves;                 // Array of waves (from Wave.cs)
    private int currentWaveIndex = 0;    // Index of the current wave

    private bool isWaveActive = false;   // Check if a wave is active
    private int enemiesRemainingToSpawn; // Number of enemies left to spawn in this wave
    private int enemiesRemainingAlive;   // Number of enemies still alive in the current wave

    public GameManager gameManager;      // Reference to the GameManager for win/lose conditions

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        // Don't start new waves if the game is over
        if (gameManager != null && gameManager.IsGameOver)
        {
            return;
        }

        if (!isWaveActive)
        {
            waveCountdown -= Time.deltaTime;
            if (waveCountdown <= 0f)
            {
                StartWave();
            }
        }

        // If wave is active and all enemies are dead, move to the next wave
        if (isWaveActive && enemiesRemainingAlive <= 0 && enemiesRemainingToSpawn <= 0)
        {
            EndWave();
        }
    }

    void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("All waves complete!");
            gameManager.TriggerVictory(); // Trigger victory condition
            return;
        }

        Debug.Log("Starting Wave " + (currentWaveIndex + 1));

        // Start the next wave
        Wave currentWave = waves[currentWaveIndex];
        enemiesRemainingToSpawn = currentWave.enemyCount;
        enemiesRemainingAlive = currentWave.enemyCount;

        isWaveActive = true;
        waveCountdown = timeBetweenWaves;

        // Start spawning enemies for the wave
        StartCoroutine(SpawnWave(currentWave));
    }

    IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            if (gameManager != null && gameManager.IsGameOver)
            {
                yield break; // Stop spawning if the game is over
            }

            SpawnEnemy(wave);
            enemiesRemainingToSpawn--;
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    void SpawnEnemy(Wave wave)
    {
        // Choose a random enemy type from the current wave's enemy types
        int randomIndex = Random.Range(0, wave.enemyTypes.Length);
        GameObject enemyPrefab = wave.enemyTypes[randomIndex];

        // Spawn the enemy
        enemySpawner.SpawnEnemy(enemyPrefab);

        Debug.Log("Enemy spawned, enemies remaining to spawn: " + enemiesRemainingToSpawn);
    }

    void EndWave()
    {
        Debug.Log("Wave " + (currentWaveIndex + 1) + " complete.");
        currentWaveIndex++;
        isWaveActive = false;
        waveCountdown = timeBetweenWaves;

        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("All waves completed!");
            gameManager.TriggerVictory(); // Trigger victory condition
        }
    }

    // Call this when an enemy dies (to decrease the remaining alive counter)
    public void EnemyDied()
    {
        enemiesRemainingAlive--;
        Debug.Log("Enemy died. Remaining enemies: " + enemiesRemainingAlive);

        // Check for victory condition (all waves completed)
        if (currentWaveIndex >= waves.Length && enemiesRemainingAlive <= 0 && enemiesRemainingToSpawn <= 0)
        {
            gameManager.TriggerVictory();
        }
    }

    // Call this when an enemy reaches the goal
    public void EnemyReachedGoal()
    {
        enemiesRemainingAlive--;
        Debug.Log("Enemy reached goal. Remaining enemies: " + enemiesRemainingAlive);
    }
}
