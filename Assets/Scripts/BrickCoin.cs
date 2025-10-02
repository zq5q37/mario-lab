using UnityEngine;

public class BrickBox : MonoBehaviour
{
    public GameObject coinPrefab;       // Coin prefab to spawn
    public AudioClip coinSound;         // Sound to play when coin spawns
    public Transform coinSpawnPoint;    // Position above the brick for coin


    private bool hasBounced = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasBounced)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRb.linearVelocity.y > 0.1f) // Hit from below
            {
                // Bounce();
                SpawnCoin();
            }
        }
    }

    void Bounce()
    {
        // Quick bounce
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);

        hasBounced = true;

    //     if (brickAnimator != null)
    //         brickAnimator.SetBool("enabled", false);

        

    //     // After short delay, stop bouncing
    //     Invoke(nameof(DisableBounce), 0.5f);
     }

    void SpawnCoin()
    {
        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);
        }

        if (coinSound != null)
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
        }
    }

    void DisableBounce()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }
}
