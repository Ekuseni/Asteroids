using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Score : MonoBehaviour
{
    static Score _instance;

    int score = 0;
    TMPro.TextMeshProUGUI scoreText;

    private void Awake()
    {
        _instance = this;
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
    }


    public static void IncreaseScore(int amount)
    {
        _instance.score += amount;

        _instance.scoreText.text = _instance.score.ToString();
    }

    public static void ResetScore()
    {
        _instance.score = 0;
        _instance.scoreText.text = _instance.score.ToString();
    }
}
