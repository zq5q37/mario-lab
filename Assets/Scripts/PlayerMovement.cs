using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class PlayerMovement : Singleton<PlayerMovement>
{
    // public float speed = 10;
    // public float maxSpeed = 20;
    // public float upSpeed = 10;
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
    private Transform gameCamera;

    //Death
    public AudioSource marioDeathAudio;
    // public float deathImpulse = 15;

    [System.NonSerialized]
    public bool alive = true;

    public Animator questionBoxAnimator;

    public UnityEvent<Void> restartEvent;

    private bool moving = false;
    private bool jumpedState = false;
    // private GameManager gameManager;


    public GameConstants gameConstants;
    float deathImpulse;
    float upSpeed;
    float maxSpeed;
    float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set constants
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        // gameOverUI.SetActive(false);
        marioAnimator.SetBool("onGround", onGroundState);
        marioSprite.flipX = false;
        // gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        SceneManager.activeSceneChanged += SetStartingPosition;
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera")?.transform;
    }

    public void SetStartingPosition(Scene current, Scene next)
    {
        if (next.name == "World-1-2")
        {
            // change the position accordingly in your World-1-2 case
            this.transform.position = new Vector3(0, 0.5f, 0.0f);
        }
    }

    // Update is called once per frame
    // Put everything that has nothing to do with physics here
    void Update()
    {
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

        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }
        if (col.gameObject.CompareTag("toad") && alive)
        {
            Debug.Log("Bounced on Toad Cloud!");


            AudioSource cloudAudio = col.gameObject.GetComponent<AudioSource>();
            if (cloudAudio != null)
            {
                cloudAudio.Play();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            float marioBottom = transform.position.y - 0.1f;
            float goombaTop = other.transform.position.y + 0.1f;

            if (marioBottom > goombaTop)
            {
                // Stomped
                var goomba = other.GetComponent<EnemyMovement>();
                if (goomba != null)
                    goomba.StopMovement(); // UnityEvent will handle score, animation, sound

                // Mario bounce
                marioBody.linearVelocity = new Vector2(0, 5f);
            }
            else
            {
                // Mario hit the Goomba from side or bottom
                marioAnimator.Play("mario-die");
                marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
                alive = false;
            }
        }
    }

    void GameOverScene()
    {

        // gameManager.GameOver();
        GameManager.instance.GameOver();
        Time.timeScale = 0.0f;
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        // if (alive)
        if (alive && moving)
        {
            Move(faceRightState ? 1 : -1);

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


    public void GameRestart()
    {
        // Always refresh reference in case the camera was destroyed
        if (gameCamera == null)
            gameCamera = GameObject.FindGameObjectWithTag("MainCamera")?.transform;

        if (gameCamera == null)
        {
            Debug.LogWarning("GameRestart: MainCamera not found!");
            return;
        }
        // reset position
        // marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        marioBody.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(5.67f, 3.5f, -10.0f);
    }
    public override void Awake()
    {
        base.Awake();
        // gameCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        // other instructions
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }
}
