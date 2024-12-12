using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the Pause Menu Panel
    private bool isPaused = false; // Keeps track of whether the game is paused

    // Start is called before the first frame update
    void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false); // Ensure the pause panel is hidden initially
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Check for the "Escape" key (to pause/unpause the game)
        {
            TogglePause();
        }
    }

    // Toggle the pause state
    public void TogglePause()
    {
        isPaused = !isPaused; // Toggle the paused state
        if (isPaused)
        {
            Time.timeScale = 0; // Pause the game by setting the time scale to 0
            pausePanel.SetActive(true); // Show the pause panel
        }
        else
        {
            Time.timeScale = 1; // Unpause the game by resetting the time scale
            pausePanel.SetActive(false); // Hide the pause panel
        }
    }

    // Method to resume the game (attached to the "Resume" button in the pause menu)
    public void ResumeGame()
    {
        TogglePause(); // Toggle the pause state (this will unpause the game)
    }

    // Method to load the main menu (attached to the "Main Menu" button in the pause menu)
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // Ensure the game is unpaused when returning to the main menu
        SceneManager.LoadScene(0); // Load the main menu scene (index 0)
    }

    // Method to quit the game (optional, attached to a Quit button in the pause menu)
    public void QuitGame()
    {
        Application.Quit(); // Quit the game
    }
}



