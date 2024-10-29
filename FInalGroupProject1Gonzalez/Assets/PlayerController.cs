using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private bool isFacingRight = true;
    private bool isDead = false;
    private int jumpCount = 0;  // Track number of jumps

    public TextMeshProUGUI gameOverText;
    public float upForce = 200f; // Adjustable in-game

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isDead) // Only allow actions if player is not dead
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            // Check if grounded to reset jump count
            if (IsGrounded())
            {
                jumpCount = 0; // Reset jumps when player touches the ground
            }

            // Double jump when pressing Space, only if jumpCount is less than 2
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
            {
                rb.velocity = new Vector2(rb.velocity.x, upForce);
                jumpCount++; // Increment jump count
            }

            // Reduce jump height when releasing Space while moving up
            if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }

            Flip();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        // This method checks if the player is on the ground layer
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        Destroy(other.gameObject);
        gameOverText.gameObject.SetActive(true);

        // Mark as dead to prevent further actions
        isDead = true;
    }
}

