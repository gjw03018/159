using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [HideInInspector]
    public Text scoreText;

    public int score;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    private void Start()
    {
        AddScore(0);
    }
    private void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            scoreText.text = score.ToString();
        }
    }
    public void AddScore(int value)
    {
        score += value;
        if (score > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        scoreText.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
    }
}
