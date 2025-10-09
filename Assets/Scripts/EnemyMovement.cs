using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    public Vector3 startPosition = Vector3.zero;

    public Animator goombaAnimator;

    [Header("Unity Events")]
    public UnityEvent onDeath; // Assign score, sound, etc. in Inspector

    public AudioSource stompAudio; // assign AudioSource in Inspector
    public AudioClip stompClip;    // assign AudioClip in Inspector

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
        velocity = new Vector2(moveRight * maxOffset / enemyPatroltime, 0);
    }

    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

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

    public void StopMovement()
    {
        velocity = Vector2.zero;

        if (enemyBody != null)
            enemyBody.linearVelocity = Vector2.zero;

        var collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        enabled = false;

        // Invoke UnityEvent to handle score, sound, animation
        onDeath?.Invoke();
    }

    public void ResumeMovement()
    {
        enabled = true;

        var collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = true;

        if (enemyBody != null)
            enemyBody.linearVelocity = Vector2.zero;

        moveRight = -1;
        originalX = transform.position.x;
        ComputeVelocity();
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

    public void PlayStompSound()
    {
        if (stompAudio != null && stompClip != null)
            stompAudio.PlayOneShot(stompClip);
    }

}
