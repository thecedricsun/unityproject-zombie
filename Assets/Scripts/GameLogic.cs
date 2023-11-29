using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public int score;
    private Text scoreText;

    // 
    [SerializeField] private GameObject _pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        // scoreText = GetComponent<Text>();
        GameObject scoreTextObject = GameObject.FindGameObjectWithTag("ScoreScript");
        if (scoreTextObject != null)
        {
            scoreText = scoreTextObject.GetComponent<Text>();
        }
        else
        {
            Debug.Log("Cannot find 'ScoreText' script");
        }

        _pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    // score add
    public void AddScore(int newScoreValue)
    {
        // UpdateScore();
        score += newScoreValue;
        scoreText.text = score.ToString();

    }

    // get score
    public int GetScore()
    {
        return score;
    }

    // reset score
    public void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
}
