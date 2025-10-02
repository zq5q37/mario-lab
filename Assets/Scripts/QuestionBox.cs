using UnityEngine;

public class QuestionBox : MonoBehaviour
{
    public Animator questionBoxAnimator;

    private bool hasBounced = false;

    private Rigidbody2D rb;

    public void resetBounce()
    {
        hasBounced = false;
        questionBoxAnimator.SetBool("enabled", true);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        resetBounce();
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
                Bounce();
            }
        }
    }

    void Bounce()
    {
        // give a quick bounce
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

        hasBounced = true;
        questionBoxAnimator.SetBool("enabled", false);
        // after short delay, turn into static
        Invoke(nameof(DisableBounce), 0.5f);
    }

    void DisableBounce()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }
}
