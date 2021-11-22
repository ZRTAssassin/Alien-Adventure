using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;

    public static int Score { get; private set; }
    static int _highScore;
    static string _highScoreKey = "HighScore";


    void Start()
    {
        _highScore = PlayerPrefs.GetInt(_highScoreKey);
        Score = 0;
    }

    public static void ResetScore()
    {
        Score = 0;
    }

    public static void Add(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
        //Debug.Log($"Score = {_score}");

        if (Score > _highScore)
        {
            _highScore = Score;
            //Debug.Log($"High Score = {_highScore}");

            PlayerPrefs.SetInt(_highScoreKey, _highScore);
        }
    }
}