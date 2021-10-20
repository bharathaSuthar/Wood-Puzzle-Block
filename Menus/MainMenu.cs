using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void HandleQuitButtonOnClickEvent(){
        Application.Quit();
        Debug.Log("Quit!");
    }
    public void HandlePlayButtonOnClickEvent(){
        SceneManager.LoadScene("GamePlay");
    }
    public void HandleOptionsButtonOnClickEvent(){
        MenuManager.GoToMenu(MenuName.Options);
        Destroy(gameObject);
    }
    
}
