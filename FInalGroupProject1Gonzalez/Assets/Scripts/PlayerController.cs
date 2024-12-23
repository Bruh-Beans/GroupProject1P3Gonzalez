using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private TimerManager timerManager;
    private float horizontal;
    private float speed = 8f;
    private bool isFacingRight = true;
    private bool isDead = false;

    public TextMeshProUGUI gameOverText;
    public float upForce = 200f; // Adjustable in-game
    bool grounded;
    public Animator animator;
    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [SerializeField] private Rigidbody2D rb;

    private bool isJumping = false; // This will track if the player is in the "jumping phase"
    private bool isGameOver = false; // Flag to check if the game is over

    private void Awake()
    {

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        {
            // Get the TimerManager script from the scene
            timerManager = FindObjectOfType<TimerManager>();
        }
    }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the player collides with an object tagged as "Win"
            if (other.CompareTag("Win"))
            {
                // Call the PlayerWins() method from the TimerManager
                if (timerManager != null)
                {
                    timerManager.PlayerWins();
                }
            }
        }

    

    void Update()
    {
        if (!isDead && !isGameOver) // Only allow actions if player is not dead and game is not over
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontal));
            horizontal = Input.GetAxisRaw("Horizontal");

            // Jumping action when pressing Space
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0, upForce), ForceMode2D.Impulse);
                animator.SetBool("IsJumping", true);
                grounded = false;

                isJumping = true; // Start the jumping phase
            }

            // Reduce jump height when releasing Space while moving up
            if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                animator.SetBool("IsJumping", true);
            }

            Flip();
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate()
    {
        if (!isGameOver) // Prevent movement if game is over
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Breakable"))
        {
            // Determine the direction of the collision
            Vector3 collisionDirection = other.GetContact(0).normal;

            // Only break the block if the player lands on top (collisionDirection == Vector3.up) and isJumping is true
            if (collisionDirection == Vector3.up && isJumping)
            {
                Animator blockAnimator = other.gameObject.GetComponent<Animator>();
                if (blockAnimator != null)
                {
                    blockAnimator.SetTrigger("PlayBreak"); // Trigger the break animation
                }

                // Destroy the block after the animation plays
                Destroy(other.gameObject, 0.15f); // Add slight delay to match animation
            }

            // Allow the player to stand on the block without breaking it
            grounded = true;
            OnLanding();
            isJumping = false;
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            // Handle landing on ground objects
            Vector3 normal = other.GetContact(0).normal;
            if (normal == Vector3.up)
            {
                grounded = true;
                OnLanding();
                isJumping = false;
            }
        }
        else if (other.gameObject.CompareTag("Win"))
        {
            // Player wins the game
            FindObjectOfType<TimerManager>().PlayerWins();
        }
    }

    private IEnumerator DestroyAfterAnimation(GameObject block)
    {
        Animator blockAnimator = block.GetComponent<Animator>();

        // Wait for the animation to finish
        if (blockAnimator != null)
        {
            AnimatorStateInfo animationState = blockAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(animationState.length); // Wait for the animation length
        }

        Destroy(block); // Destroy the block after the animation ends
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
            animator.SetBool("IsJumping", true);
        }
    }

    // Call this function when the game is over to stop the player from moving
    public void SetGameOver(bool gameOver)
    {
        isGameOver = gameOver; // Set the game over state
        if (gameOver)
        {
            // Optionally, you can show the "Game Over" UI panel here if not already shown
            // Example:
            // FindObjectOfType<TimerManager>().ActivateGameOverPanel();
        }
    }
}



