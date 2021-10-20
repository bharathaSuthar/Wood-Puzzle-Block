using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    Text scoreValue, bestScoreValue;
    string scorePrefix = "Score: ", bestScorePrefix = "Best: ";

    private void Start() {
        
    }

    public void HandlePlayAgainButtonOnClickEvent(){
        MenuManager.GoToMenu(MenuName.Main);
        Destroy(gameObject);
    }
    public void OnGameOver( int score, int best){
        scoreValue.text = scorePrefix + score;
        bestScoreValue.text = bestScorePrefix + best;
        PlayerPrefs.SetInt("HighScore", best);
    }
}
