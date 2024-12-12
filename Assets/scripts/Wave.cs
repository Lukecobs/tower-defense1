using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemyTypes;   // Array of enemy prefabs for this wave
    public int enemyCount;            // Number of enemies to spawn in this wave
    public float spawnInterval;       // Time between each enemy spawn
}
