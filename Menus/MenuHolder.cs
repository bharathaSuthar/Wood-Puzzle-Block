using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHolder : MonoBehaviour
{
    public 
    GameObject MainMenu, GameOverMenu;
    private void Start() {
        GameObject mainMenu = Instantiate(MainMenu);
        mainMenu.transform.SetParent(transform);
    }
}
