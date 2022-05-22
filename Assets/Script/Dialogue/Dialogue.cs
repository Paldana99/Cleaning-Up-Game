using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;

    private string[] lines;
    private int index;
    public bool isDialogue;
    private Image image;
    private Grid grid;
    private TilemapRenderer tilemap;

    private DialogueTrigger dialogueTrigger;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        //Time Scale = 0 y despues 1
        textComponent.text = string.Empty;
        isDialogue = false;
        image = GetComponent<Image>();
        image.enabled = false;
        grid = GetComponentInChildren<Grid>();
        tilemap = GetComponentInChildren<TilemapRenderer>();
        tilemap.enabled = false;
        dialogueTrigger = GameObject.FindObjectOfType<DialogueTrigger>();

        dialogueTrigger.StartDialogue += StartDialogue;

    }

    // Update is called once per frame
    private void Update()
    {
        if (!isDialogue) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                
            }
        }
    }

    private void StartDialogue(string[] linesReceive)
    {
        isDialogue = true;
        lines = linesReceive;
        index = 0;
        image.enabled = true;
        tilemap.enabled = true;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (var c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            isDialogue = false;
            gameObject.SetActive(false);
        }
    }
    
}
