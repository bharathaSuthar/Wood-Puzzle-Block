using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    public int score, bestScore;
    [SerializeField] Text scoreValue, bestScoreValue;
    private void Start() {
        score = 0;
        UpdateScore(score);
        if(PlayerPrefs.HasKey("HighScore")){
            bestScore = PlayerPrefs.GetInt("HighScore");
            bestScoreValue.text = bestScore.ToString();
        }
        else{
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }
    public void UpdateScore( int score ){
        this.score = score;
        scoreValue.text = score.ToString();
        if(score>=bestScore){
            bestScore = score;
            bestScoreValue.text = bestScore.ToString();
        }
    }
}
