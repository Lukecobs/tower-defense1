using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int maxEnemiesAllowed = 10; // Number of enemies allowed to reach the goal before game over

    [Header("UI Elements")]
    public GameObject gameOverScreen;  // Reference to Game Over UI
    public GameObject victoryScreen;  // Reference to Victory UI

    private int enemiesReachedGoal = 0; // Counter for enemies that reached the goal
    public bool IsGameOver { get; private set; } = false; // Track if the game is over

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnemyReachedGoal()
    {
        if (IsGameOver) return;

        enemiesReachedGoal++;

        Debug.Log($"Enemies reached goal: {enemiesReachedGoal}/{maxEnemiesAllowed}");

        if (enemiesReachedGoal >= maxEnemiesAllowed)
        {
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        if (IsGameOver) return;

        Debug.Log("Game Over!");
        IsGameOver = true;

        // Show the Game Over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;
    }

    public void TriggerVictory()
    {
        if (IsGameOver) return;

        Debug.Log("Victory!");
        IsGameOver = true;

        // Show the Victory screen
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        Time.timeScale = 1f;
        IsGameOver = false;
        enemiesReachedGoal = 0;
        SceneManager.LoadScene("Level1"); // Replace with your main game scene name
    }

    public void ExitGame()
    {
        Debug.Log("Exiting to Start Screen...");
        Time.timeScale = 1f; // Ensure the game time is normal
        IsGameOver = false;
        SceneManager.LoadScene("StartScreen"); // Replace with your start screen scene name
    }
}
