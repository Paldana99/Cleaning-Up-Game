using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private int X, Y;
    public float angle_rad = Mathf.PI/2;
    [SerializeField]
    private float speed = 1f;
    private string PLAYER_TAG = "Player";
    public Animator animator;
    public string EXPLODE_ANIMATION = "Explode";
    private CircleCollider2D circle_collider;
    public PolygonCollider2D polygon_collider;
    private Rigidbody2D rigidbody;
    public string typeAttack;
    private Vector3 targetPos;

    void Start()
    {
      setPosition(angle_rad);
      polygon_collider = GetComponent<PolygonCollider2D>();
      polygon_collider.enabled = (typeAttack != "barrier");
      rigidbody = GetComponent<Rigidbody2D>();
      targetPos = new Vector3(transform.position.x, transform.position.y+3f, 0f);
      // animator = GetComponent<Animator>();
    }


    void Update() {
      MoveSpike(typeAttack);
    }

    void MoveSpike(string attack) {
      switch (attack) {
        case "explosion":
          transform.position += new Vector3(X, Y, 0)*Time.deltaTime*speed; break;
        
        case "falling":
          break;
        
        case "barrier":
          transform.position = Vector3.MoveTowards(transform.position, targetPos,Time.deltaTime*speed);
          if (transform.position.y == targetPos.y)
            polygon_collider.enabled = true;
          break;
      }
    }


    public void setPosition(float newAngle) {
      switch(newAngle) {
        case 0f:
          X = 1; Y = 0; break;

        case Mathf.PI/4:
          X = 1; Y = 1; break;

        case Mathf.PI/2:
          X = 0; Y = 1; break;

        case Mathf.PI*3/4:
          X = -1; Y = 1; break;

        case Mathf.PI:
          X = -1; Y = 0; break;
      }

    }

    private void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.CompareTag(PLAYER_TAG) && (typeAttack != "barrier")) {
          // animator.SetBool(EXPLODE_ANIMATION, true);
          Destroy(polygon_collider);
          this.gameObject.AddComponent<CircleCollider2D>();
          circle_collider = GetComponent<CircleCollider2D>();
          for (float R = 0f; R < 0.2f; R += 0.01f) {
            circle_collider.radius = R;
            Debug.Log("hola");
            Delay(5f);
            Debug.Log("adios");
          }
          Destroy(this.gameObject);
      }

      else if ((other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Finish")) && (typeAttack != "barrier"))
        Destroy(this.gameObject);
    }

    public IEnumerator Delay(float time) {
      yield return new WaitForSeconds(time);
    }

    public void Fall(){
      rigidbody.gravityScale = 3.5f;
    }

}
