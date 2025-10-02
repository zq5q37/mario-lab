using UnityEngine;

public class QuestionBox : MonoBehaviour
{
    public Animator questionBoxAnimator;

    private bool hasBounced = false;

    public void resetBounce()
    {
        hasBounced = false;
        questionBoxAnimator.SetBool("enabled", true);
    }

    void Start()
    {
        // update animator state
        questionBoxAnimator.SetBool("enabled", true);
        hasBounced = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasBounced)
        {
            return;
        }
        // Check if the collision is from Mario/Player
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRb.linearVelocity.y > 0.1f)
            {
                hasBounced = true;
                questionBoxAnimator.SetBool("enabled", false);
            }
        }
    }
}
