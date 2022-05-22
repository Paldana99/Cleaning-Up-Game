using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HedgehogBoss : ABoss {

	[SerializeField]
	private Spike spike;

	public int maxBarrierSize, maxFallingSpikes, maxSpaces;

	protected override void Start() {
		base.Start();
		ATTACK_1_ANIMATION = "StarExplosion";
		ATTACK_2_ANIMATION = "SpikeBarrier";
		ATTACK_3_ANIMATION = "SpikeWheel";
	}

	// Star Explosion
	public override IEnumerator Attack1() {
		
		var points = polygon_collider.points;
		toCenter = false;

		// Begin attack animation
		animator.SetBool(ATTACK_1_ANIMATION, true);

		// Change of polygon_collider
		polygon_collider.points = new[]{new Vector2(0.02347305f, 0.1395289f),
										new Vector2(-0.3904575f, -0.2741727f),
										new Vector2(-0.2474838f, -0.4296466f),
										new Vector2(0.2971422f, -0.441111f),
										new Vector2(0.4474963f, -0.2863722f),
										new Vector2(0.02347305f, 0.1395289f),
										new Vector2(0.02347305f, 0.1395289f),
										new Vector2(0.02347305f, 0.1395289f),
										new Vector2(0.02347305f, 0.1395289f)};


		yield return new WaitForSeconds(1.5f);

		// Spikes creation
		float angle = 0f;
		float newX, newY;
		Vector3 newPosition;
		Spike newSpike;
		for (int i = 0; i < 5; i++) {
			newX = 2f*Mathf.Cos(angle);
			newY = 2f*Mathf.Sin(angle);
			newPosition = new Vector3(newX, newY, 0) + transform.position;
			newSpike = Instantiate(spike, newPosition, Quaternion.Euler(0, 0, angle*180/Mathf.PI));
			newSpike.GetComponent<Spike>().transform.localScale = new Vector3(20f, 20f, 0);
			newSpike.GetComponent<Spike>().angle_rad = angle;
			newSpike.GetComponent<Spike>().typeAttack = "explosion";
			angle += Mathf.PI/4;
		}

		yield return new WaitForSeconds(.75f);

		// Recoil animation
		animator.SetBool(ATTACK_1_ANIMATION, false);
		yield return new WaitForSeconds(1.5f);

		// Return to original polygon_collider
		polygon_collider.points = points;
		moveToBorder = true;
		yield break;
	}

	public override IEnumerator Attack2() {
		Vector3 newPos = new Vector3(transform.position.x + movementX*3.5f, 7.5f, 0f);
		Spike newSpike;
		int spaces = 0;
		for (int i = 0; i < maxFallingSpikes; i++) {
			float prob = Random.Range(0f, 1f);
			if (prob > .5f || spaces > maxSpaces) {
				newSpike = Instantiate(spike, newPos, Quaternion.Euler(0, 0, -90));
				newSpike.GetComponent<Spike>().transform.localScale = new Vector3(15f, 15f, 0f);
				newSpike.GetComponent<Spike>().typeAttack = "falling";
				newSpike.Invoke("Fall", .75f);
				yield return new WaitForSeconds(.375f);
			}
			else
				spaces += 1;
			newPos += new Vector3(2.25f, 0f, 0f)*movementX;
		}
		yield return new WaitForSeconds(.5f);
		isAttack = false;
	}

	public override IEnumerator Attack3() {
		Vector3 bossPos = transform.position;
		Vector3 newPos = new Vector3(bossPos.x + movementX*3.5f, bossPos.y-6.15f, 0f);
		Spike newSpike;
		Queue<Spike> barrier = new Queue<Spike>();
		
		void CreateSpike() {
			newSpike = Instantiate(spike, newPos, Quaternion.Euler(0, 0, 90));
			newSpike.GetComponent<Spike>().transform.localScale = new Vector3(20f, 20f, 0f);
			newSpike.GetComponent<Spike>().typeAttack = "barrier";
			newPos += new Vector3(3.5f, 0f, 0f)*movementX;
			barrier.Enqueue(newSpike);
		}

		for (int i = 0; i < maxBarrierSize; i++) {
			CreateSpike();
			yield return new WaitForSeconds(.375f);
		}
		
		while (Mathf.Abs(barrier.Peek().transform.position.x - Camera.main.transform.position.x) < (MAX_POSITION + 5f)) {
			Destroy(barrier.Dequeue().gameObject);
			CreateSpike();
			yield return new WaitForSeconds(.375f);
		}

		while (barrier.Count > 0) {
			Destroy(barrier.Dequeue().gameObject);
			yield return new WaitForSeconds(.375f);
		}
		
		yield return new WaitForSeconds(.5f);
		isAttack = false;
	}


}
