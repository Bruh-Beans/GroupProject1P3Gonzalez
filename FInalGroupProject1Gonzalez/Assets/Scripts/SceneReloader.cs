using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        // Register callback to run when the scene finishes loading
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unsubscribe from sceneLoaded event to avoid repeated calls
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Code here will execute once the scene is fully loaded
        // For example, you can reset the player's position or state if needed
    }
}



