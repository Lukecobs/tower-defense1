using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    // Load the main game scene
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");  // name of main game scene(case sensitive)
    }

    // Exit the application
    public void ExitGame()
    {
        Debug.Log("Game has exited!");  // For testing in the editor
        Application.Quit();             // Works only in built application
    }
}
