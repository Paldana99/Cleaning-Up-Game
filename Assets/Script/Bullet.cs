using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    private float speed = 1f;
    public bool isFlipped = false;
    private float direction;
    private Animator animator;
    private bool explosion = false;
    private SpriteRenderer spriteRenderer;
    public float DAMAGE_MULTIPLIER = 10f;
    private string GROUND_TAG = "Ground";
    private string ENEMY_TAG = "Enemy";

    void Start()
    {
      animator = GetComponent<Animator>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      setDirection();
    }

    // Update is called once per frame
    void Update()
    {
      // if (explosion) {
      //   animator.SetBool("Explosion", true);
      //   Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
      //   Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length*0.25f);
      // }
      transform.position += new Vector3(direction, 0f, 0f) * speed * Time.deltaTime;
    }


    void setDirection() {
      direction = isFlipped ? -1f : 1f;
      spriteRenderer.flipX = isFlipped;
    }

    private void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.CompareTag(GROUND_TAG) || other.gameObject.CompareTag(ENEMY_TAG)) {
        Destroy(this.gameObject);
      }


    }


}
