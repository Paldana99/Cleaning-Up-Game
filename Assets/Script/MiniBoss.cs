using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiniBoss : AEnemy
{
    [SerializeField]
    public string ATTACK_ANIMATION = "Attack";

    [SerializeField]
    private Spike spike;

    private BoxCollider2D box_collider;

    void Start() {
      life = 250f;
      animator = GetComponent<Animator>();
      animator.SetBool(ATTACK_ANIMATION, false);
      box_collider = GetComponent<BoxCollider2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      StartCoroutine(MoveRoutine());
      // AttackRoutine();
    }


    public override IEnumerator Attack() {
      animator.SetBool(ATTACK_ANIMATION, true);
      box_collider.size = new Vector2(0.16f, 0.11f);
      box_collider.offset = new Vector2(0.0f, -0.025f);
      yield return new WaitForSeconds(1.5f);
      StarExplosion();
      yield return new WaitForSeconds(1.5f);
      animator.SetBool(ATTACK_ANIMATION, false);
      isAttack = false;
      // box_collider.size = new Vector2(0.16f, 0.16f);
      // box_collider.offset = new Vector2(0.0f, 0.0f);
      yield break;
    }


    private void StarExplosion() {
      float angle = 0f;
      float newX, newY;
      Vector3 newPosition;
      Spike newSpike;
      for (int i = 0; i < 5; i++) {
        newX = 1.5f*Mathf.Cos(angle);
        newY = 1.5f*Mathf.Sin(angle);
        newPosition = new Vector3(newX, newY, 0) + transform.position;
        newSpike = Instantiate(spike, newPosition, Quaternion.Euler(0, 0, angle*180/Mathf.PI));
        newSpike.GetComponent<Spike>().angle_rad = angle;
        // newSpike.GetComponent<Spike>().isBarrier = false;
        angle += Mathf.PI/4;
      }
    }


    private IEnumerator SpikeBarrier() {
      Vector3 newPosition;
      Spike newSpike;
      List<Spike> spikeList = new List<Spike>();
      float newX = 1.1f;
      for (int i = 0; i < 10; i++) {
        newPosition = new Vector3(newX, -0.39f, 0)*movementX + transform.position;
        newSpike = Instantiate(spike, newPosition, Quaternion.Euler(0, 0, 90));
        // newSpike.GetComponent<Spike>().isBarrier = true;
        newX += 1.1f;
        spikeList.Add(newSpike);
        yield return new WaitForSeconds(0.3f);
      }
      yield return new WaitForSeconds(1f);
      for (int i = 0; i < 10; i++)
        Destroy(spikeList[i]);

    }


    // private void OnCollisionEnter2D(Collision2D other) {
    //   if (other.gameObject.CompareTag("PlayerAttack"))
    //     getDamage(other.gameObject.GetComponent<Bullet>().DAMAGE_MULTIPLIER);
    // }

    // public override void toClean() {
  	// 	if (!toxic) {
  	// 		float newScale = Mathf.Lerp(20, 5, Time.time);
  	// 		gameObject.transform.localScale = new Vector3(newScale, newScale, 0);
  	// 	}
  	// }



}
