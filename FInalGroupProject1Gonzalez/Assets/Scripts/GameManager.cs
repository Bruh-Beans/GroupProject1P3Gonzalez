using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    public Button restartButton;
    public Button startButton;
    public GameObject titleScreen;
    public bool isGameActive;
    bool gamePaused = false;
    public GameObject pauseMenuUI;

    private LockCursor lockCursor;

    void Start()
    {
        lockCursor = GameObject.Find("LockCursorManager").GetComponent<LockCursor>();
        lockCursor.Unlock(); // Unlock cursor when on title screen
    }

    void Update()
    {
        PauseMenu();
    }


    public void StartGame(int difficulty)
    {
        isGameActive = true; // Set game active when game starts
        titleScreen.SetActive(false);
        lockCursor.Lock(); // Lock cursor when the game starts
    }
    void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gamePaused)
            {
                gamePaused = false;
                Time.timeScale = 1f;
                pauseMenuUI.SetActive(false);
                isGameActive = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && isGameActive)
            {
                if (!gamePaused)
                {

                    Time.timeScale = 0f;
                    pauseMenuUI.SetActive(true);
                    gamePaused = true;
                    isGameActive = false;
                }

            }
        }
    }
}
