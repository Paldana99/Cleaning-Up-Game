using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public string scene;
    public bool debug = false;
    public Animator crossFade;
    private LevelSelection levelManager;
    private GameObject check;
    private AudioManager controller;
    
    void Awake() {
        if (!debug) {
            check = GameObject.Find("__app");
            if (check == null)
                UnityEngine.SceneManagement.SceneManager.LoadScene("_preload");
            else {
                controller = check.GetComponent<AudioManager>();
                controller.StopMusic();
                StartCoroutine(PlayMusic());
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayMusic());

        var specialScenes = new[] {"Boss", "Tutorial"};
        var nameScene = SceneManager.GetActiveScene().name;
        if (!specialScenes.Any(nameScene.Contains))
        {
            levelManager = FindObjectOfType<LevelSelection>();
            if (levelManager != null) scene = levelManager.getActualBoss();
        }        
    }

    public void StartLoad() {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene() {
        crossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator PlayMusic() {
        controller.source_begin.Play();
        yield return new WaitForSeconds(controller.source_begin.clip.length - 0.5f);
        controller.source_level.Play();
    }
}
