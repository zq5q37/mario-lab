using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;

    private int score = 0;
    public static IntVariable gameScore;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score
        score = 0;
        SetScore(score);
        ResetAllQuestionBoxes();
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    private void ResetAllQuestionBoxes()
    {
        // Find all active question boxes in the scene
        QuestionBox[] boxes = FindObjectsByType<QuestionBox>(FindObjectsSortMode.None);
        foreach (var box in boxes)
        {
            box.resetBounce();
        }
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        SetScore(score);
    }

    public void SetScore(int score)
    {
        scoreChange.Invoke(score);
    }


    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }

}