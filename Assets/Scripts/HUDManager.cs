using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HUDManager : Singleton<HUDManager>
{
    private Vector3[] scoreTextPosition = { new Vector3(-830, 470, 0), new Vector3(-200, 0, 0) };
    private Vector3[] restartButtonPosition =
    {
        new Vector3(800, 400, 0),
        new Vector3(-50, -200, 0),
    };

    public GameObject scoreText;
    public GameObject gameOverText;
    public Transform restartButton;

    public GameObject Panel;

    public IntVariable gameScore;

    [SerializeField]
    // private GameObject pauseMenu;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void GameStart()
    {
        // hide gameover panel
        Panel.SetActive(false);
        gameOverText.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
        // Debug.Log("Game Start");
    }

    public void SetScore(int newScore)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + newScore.ToString();
        // Debug.Log("Score: " + score.ToString());
    }

    public void GameOver()
    {
        Panel.SetActive(true);
        gameOverText.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];
        // Debug.Log("Game Over");
    }

    void ShowPauseMenu()
    {
        Panel.SetActive(true);
    }

    void HidePauseMenu()
    {
        Panel.SetActive(false);
    }

    public override void Awake()
    {
        // Make sure the singleton setup runs
        base.Awake();

        // Then add your custom initialization
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(GameOver);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);
        GameManager.instance.gamePaused.AddListener(ShowPauseMenu);
        GameManager.instance.gameResumed.AddListener(HidePauseMenu);
    }
}
