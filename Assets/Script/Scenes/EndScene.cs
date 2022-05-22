using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{

    private const string PlayerTag = "Player";

    private SceneController manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(PlayerTag))
        {
            Debug.Log("End Level");
            manager.StartLoad();
        }
    }
}
