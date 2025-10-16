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

    // private int score = 0;
    public IntVariable gameScore;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        gameScore.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score
        // score = 0;
        // SetScore(score);
        gameScore.Value = 0;
        SetScore(gameScore.Value);
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
        gameScore.ApplyChange(increment);
        SetScore(gameScore.Value);
        // score += increment;
        // SetScore(score);
    }

    public void SetScore(int score)
    {
        // scoreChange.Invoke(score);
        scoreChange.Invoke(gameScore.Value);
    }


    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }

}