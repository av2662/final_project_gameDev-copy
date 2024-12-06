using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private bool isPaused = false; // Track the pause state

    [SerializeField] private bool closedByDefault = true; // Whether the canvas is closed initially

    void Awake()
    {
        if (closedByDefault)
        {
            GetComponent<Canvas>().enabled = false; // Disable canvas on start
        }
    }

    // Call this method when the Pause button is clicked
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame(); // Resume if already paused
        }
        else
        {
            PauseGame(); // Pause if not already paused
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze game time
        GetComponent<Canvas>().enabled = true; // Show pause menu
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time
        GetComponent<Canvas>().enabled = false; // Hide pause menu
    }

    public void GoBackToMainMenu()
    {
        // Save the current level
        PlayerPrefs.SetString("SavedLevel", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        // Reset time scale and load main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
