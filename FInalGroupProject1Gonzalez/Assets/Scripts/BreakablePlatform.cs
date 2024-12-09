using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    private bool isPlayerOnPlatform = false; // Tracks if the player is on the platform

    // Called when another object enters the collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true; // Player is on the platform
        }
    }

    // Called when another object exits the collider
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false; // Player is no longer on the platform
        }
    }

    private void Update()
    {
        // Check if the player is on the platform and presses the jump button
        if (isPlayerOnPlatform && (Input.GetKeyDown(KeyCode.Space)))
        {
            Destroy(gameObject); // Destroy the platform
        }
    }
}


