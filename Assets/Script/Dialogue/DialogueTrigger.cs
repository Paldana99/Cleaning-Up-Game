using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    public delegate void StartDialogueEvent(string[] lines);

    private const string PlayerTag = "Player";

    public string[] lines;
    public event StartDialogueEvent StartDialogue;

    private BoxCollider2D collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(PlayerTag))
        {
            if (StartDialogue != null) StartDialogue(lines);
            collider.enabled = false;
        }
    }
}

