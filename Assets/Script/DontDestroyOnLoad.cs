using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField]
    private string scene;


    public void Awake() {
      DontDestroyOnLoad(gameObject);
    }

    public void Start() {
      SceneManager.LoadScene(scene);
      Debug.Log("Start called");
    }
}
