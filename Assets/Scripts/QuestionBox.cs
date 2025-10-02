using UnityEngine;

public class QuestionBox : MonoBehaviour
{
    // [Header("Bounce Settings")]
    // [SerializeField] private float bounceHeight = 0.5f;
    // [SerializeField] private float bounceDuration = 0.2f;

    public Animator questionBoxAnimator;

    private Vector3 originalPos;
    private bool isBouncing = false;
    private SpringJoint2D springJoint;

    // public Rigidbody2D marioBody;

    void Start()
    {
        originalPos = transform.position;
        springJoint = GetComponent<SpringJoint2D>();
        // update animator state
        questionBoxAnimator.SetBool("enabled", true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is from Mario/Player
        if (collision.gameObject.CompareTag("Player") && !isBouncing)
        {
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            // Get the collision normal to determine hit direction
            // ContactPoint2D contact = collision.GetContact(0);
            if (playerRb.linearVelocity.y > 0.1f)
            {
                questionBoxAnimator.SetBool("enabled", false);
            }


            // If the normal points downward, Mario hit from below
            // Normal vector points away from the surface that was hit
            // if (contact.normal.y < -0.5f) // Hitting from below
            // {
            //     TriggerBounce();


            // }
            // else if (contact.normal.y > 0.5f) // Mario landing on top
            // {
            //     // Ignore collision from top
            //     Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            // }
        }
    }

    // void TriggerBounce()
    // {
    //     if (!isBouncing)
    //     {
    //         StartCoroutine(BounceAnimation());
    //     }
    // }

    // System.Collections.IEnumerator BounceAnimation()
    // {
    //     isBouncing = true;

    //     // Store original spring joint anchor
    //     Vector2 originalAnchor = Vector2.zero;
    //     if (springJoint != null)
    //     {
    //         originalAnchor = springJoint.connectedAnchor;
    //     }

    //     Vector2 targetAnchor = originalAnchor + Vector2.up * bounceHeight;
    //     float elapsed = 0f;

    //     // Bounce up - move the spring joint anchor
    //     while (elapsed < bounceDuration / 2)
    //     {
    //         if (springJoint != null)
    //         {
    //             springJoint.connectedAnchor = Vector2.Lerp(originalAnchor, targetAnchor, elapsed / (bounceDuration / 2));
    //         }
    //         elapsed += Time.deltaTime;
    //         yield return null;
    //     }

    //     elapsed = 0f;

    //     // Bounce down - return the spring joint anchor
    //     while (elapsed < bounceDuration / 2)
    //     {
    //         if (springJoint != null)
    //         {
    //             springJoint.connectedAnchor = Vector2.Lerp(targetAnchor, originalAnchor, elapsed / (bounceDuration / 2));
    //         }
    //         elapsed += Time.deltaTime;
    //         yield return null;
    //     }

    //     // Restore original anchor position
    //     if (springJoint != null)
    //     {
    //         springJoint.connectedAnchor = originalAnchor;
    //     }

    //     isBouncing = false;
    // }
}