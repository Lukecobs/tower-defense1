using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the pause menu UI
    private bool isPaused = false; // Keeps track of whether the game is paused

    void Update()
    {
        // Toggle pause when the player presses the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu
        Time.timeScale = 0f;         // Freeze the game
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f;          // Resume the game
        isPaused = false;
    }

    public void ExitGame()
    {
        Debug.Log("Exiting to main menu...");
        Time.timeScale = 1f;          // Ensure the game time is normal
        SceneManager.LoadScene("StartScreen"); // Replace with your start screen scene name
    }
}
