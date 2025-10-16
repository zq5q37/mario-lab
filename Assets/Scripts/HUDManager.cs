using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : Singleton<HUDManager>
{
    private Vector3[] scoreTextPosition = { new Vector3(-830, 470, 0), new Vector3(-200, 0, 0) };
    private Vector3[] restartButtonPosition = { new Vector3(800, 400, 0), new Vector3(-50, -200, 0) };

    public GameObject scoreText;
    public Transform restartButton;

    public GameObject gameOverPanel;

    public IntVariable gameScore;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void GameStart()
    {
        // hide gameover panel
        gameOverPanel.SetActive(false);
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
        gameOverPanel.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];
        // Debug.Log("Game Over");
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
    }
}
