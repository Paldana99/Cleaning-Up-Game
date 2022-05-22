using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  private GameObject check;
  public bool debug = false;
  private AudioManager controller;
  private string scene_name;


  private void Awake() {
    if (!debug) {
      check = GameObject.Find("__app");
      if (check == null)
        UnityEngine.SceneManagement.SceneManager.LoadScene("_preload");
      else {
        controller = check.GetComponent<AudioManager>();
        scene_name = SceneManager.GetActiveScene().name;
        controller.StopMusic();
        controller.source_intro.Play();
      }
    }
  }
}
