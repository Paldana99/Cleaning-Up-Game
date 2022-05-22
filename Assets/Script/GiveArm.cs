using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveArm : MonoBehaviour
{
    private const string AnimARM = "Arm";
    private bool dialogueStart;
    private Dialogue dialogue;
    private Animator animator;
    [SerializeField]
    private Animator playerAnimator;
    private MainVar mainVar;

    // Start is called before the first frame update
    void Start()
    {
        dialogueStart = false;
        dialogue = GameObject.FindObjectOfType<Dialogue>();
        animator = gameObject.GetComponentInParent<Animator>();
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        mainVar = GameObject.FindObjectOfType<MainVar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogue.isDialogue == true && !dialogueStart)
        {
            dialogueStart = true;
        }
        if (!dialogue.isDialogue && dialogueStart)
        {
            animator.SetBool(AnimARM, false);
            playerAnimator.SetBool(AnimARM, true);
            mainVar.isArmed = true;
            dialogueStart = false;
        }
    }
}
