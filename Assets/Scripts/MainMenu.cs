using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject highScoreText;
    public GameObject resetHighScoreButton;
    public IntVariable gameScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setHighScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void setHighScore()
    {
        // PlayerPrefs.SetInt("HighScore", 0);
        highScoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + gameScore.previousHighestValue.ToString();
    }

    public void ResetHighScore()
    {
        gameScore.ResetHighestValue();
        setHighScore();
    }
}
