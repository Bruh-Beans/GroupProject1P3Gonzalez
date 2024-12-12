using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float countdownTime = 60f; // Initial countdown time in seconds
    public TextMeshProUGUI timerText; // Timer UI Text
    public GameObject gameOverPanel; // Game Over UI Panel (set inactive in the editor)
    public GameObject winPanel; // Win UI Panel (set inactive in the editor)
    public TextMeshProUGUI winTimeText; // Win Panel Time Display
    private bool isTimerRunning = true; // Is the timer active
    private float bestTime; // Stores the player's best time
    private static TimerManager instance; // Singleton instance for persistence
    public GameObject canvas; // Reference to the Canvas (set as a child of TimerManager in the editor)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Preserve the TimerManager across scenes
            canvas.SetActive(true); // Ensure the canvas is active when TimerManager is initialized
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate TimerManager instances
        }
    }

    private void Start()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime", 0f); // Load saved best time
        SetupUI(); // Ensure UI elements are set up in the current scene
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            countdownTime -= Time.deltaTime;

            if (countdownTime <= 0)
            {
                countdownTime = 0;
                EndGame(); // Calls Game Over logic
            }

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(countdownTime / 60);
            int seconds = Mathf.FloorToInt(countdownTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void EndGame()
    {
        isTimerRunning = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Activate the Game Over panel
        }
        else
        {
            Debug.LogError("Game Over Panel is not assigned or missing in the scene!");
        }
    }

    public void PlayerWins()
    {
        isTimerRunning = false;

        if (winPanel != null)
        {
            winPanel.SetActive(true); // Activate the Win panel

            // Display the player's time
            int minutes = Mathf.FloorToInt(countdownTime / 60);
            int seconds = Mathf.FloorToInt(countdownTime % 60);
            winTimeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
        else
        {
            Debug.LogError("Win Panel is not assigned or missing in the scene!");
        }

        // Save the player's best time
        if (countdownTime < bestTime || bestTime == 0)
        {
            bestTime = countdownTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }

    public void ReturnToMainMenu()
    {
        // Destroy TimerManager object when going back to the main menu
        Destroy(gameObject); // This will destroy the TimerManager, so it is reinitialized when play starts again
        SceneManager.LoadScene(0); // Load the title screen (Scene 0)
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene load events
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupUI(); // Ensure UI elements are reconnected in the new scene
    }

    private void SetupUI()
    {
        // Reassign the timer UI and check for Game Over/Win panels in the scene
        timerText = FindObjectOfType<TextMeshProUGUI>();

        if (gameOverPanel == null)
        {
            gameOverPanel = GameObject.FindWithTag("GameOverPanel");
        }

        if (winPanel == null)
        {
            winPanel = GameObject.FindWithTag("WinPanel");
        }
    }
}



