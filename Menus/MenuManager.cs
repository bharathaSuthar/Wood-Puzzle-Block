using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum MenuName{
    Main, Pause, Options, Highscore, GameOver
}
public static class MenuManager
{
    
    public static void GoToMenu(MenuName menuName){
        GameObject menuHolder = GameObject.Find("MenuHolder");
        switch(menuName){
            case MenuName.Main:
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1;
                break;
            // case MenuName.Pause:
            //     GameObject pauseMenu = (GameObject)GameObject.Instantiate(Resources.Load("Pause Menu Canvas"));
            //     //pauseMenu.transform.SetParent(menuHolder.transform);
            //     break;
            // case MenuName.Highscore:
            //     GameObject highscoreMenu = (GameObject)GameObject.Instantiate(Resources.Load("Highscore Menu Canvas"));
            //     highscoreMenu.transform.SetParent(menuHolder.transform);
            //     break;
            // case MenuName.Options:
            //     GameObject optionsMenu = (GameObject)GameObject.Instantiate(Resources.Load("Options Menu Canvas"));
            //     optionsMenu.transform.SetParent(menuHolder.transform);
            //     break;
            case MenuName.GameOver:
                GameObject gameOverMenu = (GameObject)GameObject.Instantiate(Resources.Load("Game Over Menu"));
                gameOverMenu.transform.parent = menuHolder.transform;
                break;
        }
    }
}
