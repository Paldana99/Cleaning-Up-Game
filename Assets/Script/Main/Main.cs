using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    private Rigidbody2D myBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private UIManagerLevel manager;
    [SerializeField]
    private string WALK_ANIMATION = "Walk";
    private string JUMP_ANIMATION = "Jump";
    private const string AnimARM = "Arm";
    [SerializeField]
    private float moveForce = 10f;
    [SerializeField]
    private float jumpForce = 7f;
    [SerializeField]
    private Bullet bullet;

    private float movementX;

    private string BLOCK_SCENE_BOSS = "Block";
    private string GROUND_TAG = "Ground";
    private string ENEMY_TAG = "Enemy";
    private string END_LEVEL_TAG = "EndLevel";
    private bool isGrounded = true;
    private bool isAttack = false;

    private FlashDamage flashDamage;

    public SceneController Manager;

    public int lives = 5;

    //Combat
    private MainAttackMelee mainAttackMelee;

    // Dialogue
    private Dialogue dialogue;
    private static readonly string Death = "Death";
    private MainVar mainVar;


    private void Awake() {

        myBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashDamage = GetComponent<FlashDamage>();
        mainAttackMelee = GetComponent<MainAttackMelee>();
        dialogue = GameObject.FindObjectOfType<Dialogue>();
        
    }

    void Start()
    {
        mainVar = GameObject.FindObjectOfType<MainVar>();
        animator.SetBool(AnimARM, mainVar.isArmed);
    }

    // Update is called once per frame
    void Update()
    {
        AnimatePlayer();
        if (dialogue)
        {
            if (dialogue.isDialogue)
            {
                movementX = 0;
                return; // If there is a dialogue, don't move or attack
            }
        }
        PlayerMoveKeyBoard();
        PlayerAttack();
        mainAttackMelee.Combos();
    }

    private void FixedUpdate() {
        PlayerJump();
    }

    void PlayerMoveKeyBoard() {
        movementX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime * moveForce;
    }

    void AnimatePlayer() {
        if (movementX > 0) {
            animator.SetBool(WALK_ANIMATION, true);
            spriteRenderer.flipX = false;
        } else if (movementX < 0) {
            animator.SetBool(WALK_ANIMATION, true);
            spriteRenderer.flipX = true;
        } else {
            animator.SetBool(WALK_ANIMATION, false);
        }
    }

    void PlayerJump() {
        if (Input.GetButtonDown("Jump") && isGrounded) {
            isGrounded = false;
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetBool(JUMP_ANIMATION, true);
        }
    }

    void PlayerAttack() {
      if (Input.GetKeyDown("z") && !isAttack) {
        isAttack = true;
        Vector3 newPosition = transform.position + new Vector3(0, -0.3f, 0);
        Bullet newBullet = Instantiate(bullet, newPosition, Quaternion.Euler(0, 0, 0));
        newBullet.GetComponent<Bullet>().isFlipped = spriteRenderer.flipX;
        isAttack = false;
      }
    }

    private IEnumerator GetDamage()
    {
        flashDamage.Flash();
        lives -= 1;
        if (lives < 1)
        {
            manager.updateLives(lives);
            animator.SetTrigger(Death);
            // Wait for animation to end lenght 1sec.
            yield return new WaitForSeconds(1.1f);
            Destroy(this.gameObject);
            yield return 0;
        }
        manager.updateLives(lives);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag(GROUND_TAG)) {
            // Debug.Log("Grounded");
            isGrounded = true;
            animator.SetBool(JUMP_ANIMATION, false);
        }
        else if (other.gameObject.CompareTag(ENEMY_TAG))
        {
            StartCoroutine(GetDamage());
        }
        else
        {
            isGrounded = false;
        }
    }

}
