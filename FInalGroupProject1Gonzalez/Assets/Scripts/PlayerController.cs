using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
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

    private void Awake()
    {

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    void Update()
    {
        if (!isDead) // Only allow actions if player is not dead
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
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
        if (isJumping && other.gameObject.CompareTag("Breakable"))
        {
            // Get the Animator component of the Breakable block
            Animator blockAnimator = other.gameObject.GetComponent<Animator>();

            if (blockAnimator != null)
            {
                blockAnimator.SetTrigger("PlayBreak"); // Trigger the block's animation
            }

            // Destroy the block after the animation plays (adjust timing as needed)
            Destroy(other.gameObject, 0.15f); // Add a slight delay to match animation length
            isJumping = false; // End the jumping phase
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Vector3 normal = other.GetContact(0).normal;
            if (normal == Vector3.up) // Ensure the player lands on the ground
            {
                grounded = true;
                OnLanding();
                isJumping = false;
            }
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
}

