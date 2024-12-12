using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float speed;   // Movement speed
    public int health;    // Health points
    public int damage;    // Damage dealt to the player (reserved for future player health feature)
    public int rewardAmount = 10; // Amount of money given when this enemy dies

    private Transform[] waypoints;  // Waypoints the enemy will follow
    private int waypointIndex = 0;  // Current waypoint index
    private WaveManager waveManager;  // Reference to the WaveManager

    void Start()
    {
        // Initialize reference to the WaveManager
        waveManager = FindObjectOfType<WaveManager>();
    }

    // Set the waypoints when the enemy is spawned
    public void Initialize(Transform[] pathWaypoints)
    {
        waypoints = pathWaypoints;
        transform.position = waypoints[0].position;  // Start at the first waypoint
        waypointIndex = 1;  // Start moving towards the second waypoint
    }

    void Update()
    {
        Move();
    }

    // Handle enemy movement towards waypoints
    void Move()
    {
        if (waypointIndex < waypoints.Length)
        {
            // Move towards the next waypoint
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.deltaTime);

            // If the enemy reaches the current waypoint, move to the next one
            if (Vector2.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
            {
                waypointIndex++;
            }
        }
        else
        {
            ReachGoal();  // Call the ReachGoal method when the enemy reaches the end
        }
    }

    // This method is called when the enemy reaches the player's goal
    protected virtual void ReachGoal()
    {
        Debug.Log("Enemy reached the goal and is destroyed.");

        // Notify the WaveManager that an enemy has "died."
        if (waveManager != null)
        {
            waveManager.EnemyDied();
        }

        // Notify the GameManager for lose condition
        GameManager.Instance.EnemyReachedGoal();

        // Destroy the enemy once it reaches the goal
        Destroy(gameObject);
    }


    // Take damage
    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    // Handle the enemy death
    protected virtual void Die()
    {
        Debug.Log("Enemy died.");

        // Notify the WaveManager that an enemy has died
        if (waveManager != null)
        {
            waveManager.EnemyDied();
        }

        // Add currency reward to the player
        CurrencyManager.Instance.EarnMoney(rewardAmount);

        // Destroy the enemy
        Destroy(gameObject);
    }

 

}
