using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);

    public Animator goombaAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localPosition = startPosition;
        enemyBody = GetComponent<Rigidbody2D>();
        originalX = transform.position.x;
        ComputeVelocity();
        goombaAnimator = GetComponent<Animator>();
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    // }

    void FixedUpdate()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {
            Movegoomba();
        }
        else
        {
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    public void GameRestart()
    {
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
        goombaAnimator.SetTrigger("gameRestart");
        ResumeMovement();
    }

    public void StopMovement()
    {
        // Stop all horizontal movement
        velocity = Vector2.zero;

        // Stop physics updates
        if (enemyBody != null)
        {
            enemyBody.linearVelocity = Vector2.zero; // remember: you use linearVelocity
            // enemyBody.bodyType = RigidbodyType2D.Kinematic;
        }

        // Disable collider so Mario can pass through
        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Optionally, stop updating movement
        enabled = false;
    }

    public void ResumeMovement()
    {
        // Reactivate script
        enabled = true;

        // Re-enable collider
        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        // Reactivate physics
        if (enemyBody != null)
        {
            // enemyBody.bodyType = RigidbodyType2D.Dynamic;
            enemyBody.linearVelocity = Vector2.zero;
        }

        // Recompute movement variables
        moveRight = -1;
        originalX = transform.position.x;
        ComputeVelocity();
    }



}
