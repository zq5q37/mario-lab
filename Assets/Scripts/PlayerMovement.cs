using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private bool onGroundState = true;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // public TextMeshProUGUI scoreText;
    // public TextMeshProUGUI gameOverScoreText;
    public GameObject enemies;

    // public JumpOverGoomba jumpOverGoomba;
    // public GameObject inGameUI;
    // public GameObject gameOverUI;

    public Animator marioAnimator;
    public Transform gameCamera;

    //Death
    public AudioClip marioDeath;
    public float deathImpulse = 15;

    [System.NonSerialized]
    public bool alive = true;

    public Animator questionBoxAnimator;

    public UnityEvent<Void> restartEvent;

    private bool moving = false;
    private bool jumpedState = false;
    public GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        // gameOverUI.SetActive(false);
        marioAnimator.SetBool("onGround", onGroundState);
        marioSprite.flipX = false;
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    // Put everything that has nothing to do with physics here
    void Update()
    {
        // if (Input.GetKeyDown("a") && faceRightState)
        // {
        //     faceRightState = false;
        //     marioSprite.flipX = true;
        //     if (marioBody.linearVelocity.x > 0.1f)
        //         marioAnimator.SetTrigger("onSkid");
        // }
        // if (Input.GetKeyDown("d") && !faceRightState)
        // {
        //     faceRightState = true;
        //     marioSprite.flipX = false;
        //     if (marioBody.linearVelocity.x < -0.1f)
        //         marioAnimator.SetTrigger("onSkid");
        // }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    void OnCollisionEnter2D(Collision2D col)
    {
        // if (
        //     col.gameObject.CompareTag("Ground")
        //     || col.gameObject.CompareTag("Enemies")
        //     || col.gameObject.CompareTag("Obstacles") && !onGroundState
        // )
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            Debug.Log("Collided with Goomba!");

            marioAnimator.Play("mario-die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }

    void GameOverScene()
    {
        // gameOverScoreText.text = scoreText.text;
        // inGameUI.SetActive(false);
        // gameOverUI.SetActive(true);
        gameManager.GameOver();
        Time.timeScale = 0.0f;
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        // if (alive)
        if (alive && moving)
        {
            Move(faceRightState ? 1 : -1);
            // float moveHorizontal = Input.GetAxisRaw("Horizontal");

            // if (Mathf.Abs(moveHorizontal) > 0)
            // {
            //     Vector2 movement = new Vector2(moveHorizontal, 0);
            //     if (marioBody.linearVelocity.magnitude < maxSpeed)
            //     {
            //         marioBody.AddForce(movement * speed);
            //     }
            // }

            // // if lift key, stop moving
            // if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            // {
            //     marioBody.linearVelocity = Vector2.zero;
            // }

            // if (Input.GetKeyDown("space") && onGroundState)
            // {
            //     marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            //     onGroundState = false;
            //     marioAnimator.SetBool("onGround", onGroundState);
            // }
        }
    }

    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    // for audio
    public AudioSource marioAudio;

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }


    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }



    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    public void RestartButtonCallback(int input)
    {
        GameRestart();
        Time.timeScale = 1.0f;
    }

    // private void ResetGame()
    // {
    //     restartEvent.Invoke(null);
    //     marioBody.transform.position = new Vector3(-34.8f, 0.5f, 0.0f);
    //     faceRightState = true;
    //     marioSprite.flipX = false;
    //     scoreText.text = "Score: 0";
    //     foreach (Transform eachChild in enemies.transform)
    //     {
    //         eachChild.transform.localPosition = eachChild
    //             .GetComponent<EnemyMovement>()
    //             .startPosition;
    //     }
    //     jumpOverGoomba.score = 0;
    //     inGameUI.SetActive(true);
    //     gameOverUI.SetActive(false);

    //     marioAnimator.SetTrigger("gameRestart");
    //     alive = true;
    //     // reset camera position
    //     gameCamera.position = new Vector3(-31.1f, 3.5f, -10);
    //     questionBoxAnimator.SetBool("enabled", true);
    // }

    public void GameRestart()
    {
        // reset position
        // marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        marioBody.transform.position = new Vector3(-34.8f, 0.5f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(-31.1f, 3.5f, -10);
    }
}
