using System.Collections;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public Animator blockAnimator;  // Reference to the Animator component
    public string breakAnimationTrigger = "PlayBreak";  // The trigger name for the break animation
    public float destructionDelay = 1f;  // Delay before destroying the block after animation

    private void Start()
    {
        if (blockAnimator == null)
        {
            // Attempt to find the Animator component on the same GameObject
            blockAnimator = GetComponent<Animator>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding with the block is tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Play the break animation using the "PlayBreak" trigger
            blockAnimator.SetTrigger(breakAnimationTrigger);

            // Start the destruction process with a delay
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for the duration of the break animation before destroying the block
        yield return new WaitForSeconds(destructionDelay);

        // Destroy the block GameObject
        Destroy(gameObject);
    }
}
