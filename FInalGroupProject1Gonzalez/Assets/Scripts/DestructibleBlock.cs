using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBlock : MonoBehaviour
{
    public float breakForceThreshold = -5f; // The velocity required to break the block (negative for downward motion)

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            // Check if the player's vertical velocity is below the break threshold
            if (rb.velocity.y <= breakForceThreshold)
            {
                Destroy(gameObject); // Destroy the block
            }
        }
    }
}
