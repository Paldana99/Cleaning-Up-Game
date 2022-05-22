using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AEnemy : MonoBehaviour {

    public float speed;
    protected Rigidbody2D myBody;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Collider2D collider;
    protected int movementX = 1;
    [SerializeField]
    private string WALK_ANIMATION = "Walk";
    private string TOXIC_ANIMATION = "Toxic";
    public bool toxic = true;
    public bool isAttack = false;
    [SerializeField]
    public float life;
    private int CLEAN_LAYER = 11;

    private void Start() {
        myBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        StartCoroutine( MoveRoutine() );
    }

    public IEnumerator MoveRoutine() {
        while (animator.GetBool("Toxic"))
        {
            Debug.Log("Moving");
            yield return new WaitForSeconds(0.1f);
            float r = Random.Range(0f, 1f);
            // Debug.Log(r);
            if (r < 0.05) {
                animator.SetBool(WALK_ANIMATION, false);
                yield return new WaitForSeconds(1f);
            } else
            {
                animateEnemy();
                moveEnemy();
            }
            float p = Random.Range(0f, 1f);
            if (!isAttack) {
              isAttack = true;
              Debug.Log("Attacking");
              animator.SetBool(WALK_ANIMATION, false);
              StartCoroutine(Attack());
            }
        }

    }

    public void Update() {
    }

    private void moveEnemy(){
        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime * speed;
    }

    private void animateEnemy(){
        if (movementX == 0)
        {
            animator.SetBool(WALK_ANIMATION, false);
        } else if (movementX > 0) {
            animator.SetBool(WALK_ANIMATION, true);
            spriteRenderer.flipX = true;
        } else {
            animator.SetBool(WALK_ANIMATION, true);
            spriteRenderer.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("turn"))
        {
            if (movementX == 1)
            {
                movementX = -1;
            } else
            {
                movementX = 1;
            }
        }
    }


    // private void OnCollisionEnter2D(Collision2D other) {
    //   if (other.gameObject.CompareTag("PlayerAttack"))
    //     getDamage(other.gameObject.GetComponent<Bullet>().DAMAGE_MULTIPLIER);
    // }

    public abstract IEnumerator Attack();
}
