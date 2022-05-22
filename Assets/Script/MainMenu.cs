using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : Menu
{
    public string level;

    public void PlayGame() {
      SceneManager.LoadScene(level);
    }


    public void QuitGame() {
      Application.Quit();
    }


    public void GoToMenu() {
      confirmationBox.SetActive(false);
      panel.SetActive(true);
    }
}
