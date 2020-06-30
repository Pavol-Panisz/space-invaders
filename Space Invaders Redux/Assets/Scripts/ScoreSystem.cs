using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    private static int score = 0;
    Text scoreText;

    
    private void Awake()
    {
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
    }

    private void Start()
    {
        UpdateScoreText();
    }

    public int GetScore() { return score; }
    public void IncreaseScore(int n) { 
        score += n;
        UpdateScoreText();
    }
    public void ResetScore() { score = 0; }
    private void UpdateScoreText() { 
        if (scoreText)
        {
            scoreText.text = score.ToString();
        }
    }
}
