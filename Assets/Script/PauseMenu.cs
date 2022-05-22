using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    public static bool isPaused = false;
    public GameObject screenObjects;
    public GameObject app;
    private AudioSource source;


    public void Start() {
      app = GameObject.Find("__app");
      source = app.GetComponent<AudioManager>().source_level;
    }

    void Update()
    {
      if (Input.GetKeyDown("p") || Input.GetKeyDown(KeyCode.Escape)) {
        if (isPaused)
          ResumeGame();
        else
          PauseGame();
      }
    }


    public void ResumeGame() {
      isPaused = false;
      Time.timeScale = 1f;
      panel.SetActive(false);
      confirmationBox.SetActive(false);
      screenObjects.SetActive(true);
      source.volume = 1f;
    }


    public void PauseGame() {
      isPaused = true;
      Time.timeScale = 0f;
      panel.SetActive(true);
      confirmationBox.SetActive(false);
      screenObjects.SetActive(false);
      source.volume = 0.25f;
    }

    public void ResetGame(){
      Time.timeScale = 1f;
      source.volume = 1f;
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void MainMenu() {
      Time.timeScale = 1f;
      source.volume = 1f;
      SceneManager.LoadScene("HomePage");
    }

}
