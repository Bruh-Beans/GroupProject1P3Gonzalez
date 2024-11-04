using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private bool isDead = false;

    public TextMeshProUGUI gameOverText;
    public float upForce = 200f; // Adjustable in-game
    bool grounded;
    public Animator animator;

    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        Destroy(other.gameObject);
        gameOverText.gameObject.SetActive(true);

        // Mark as dead to prevent further actions
        isDead = true;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Vector3 normal = other.GetContact(0).normal;
            if(normal == Vector3.up)
            {
                grounded = true;
            }

        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
