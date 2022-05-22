using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ABoss : MonoBehaviour {

	public float speed;
	protected Rigidbody2D myBody;
	protected Animator animator;
	protected SpriteRenderer spriteRenderer;
	protected PolygonCollider2D polygon_collider;
	protected CircleCollider2D circle_collider;
	protected int movementX = -1;
	protected string WALK_ANIMATION = "Walk";
	protected string TOXIC_ANIMATION = "Toxic";
	private EnemyDamage enemyDamage;
	protected string ATTACK_1_ANIMATION, ATTACK_2_ANIMATION, ATTACK_3_ANIMATION;
	public bool isAttack = false;
	protected bool toCenter = false;
	protected bool moveToBorder = false;
	private int CLEAN_LAYER = 11;
	private AudioSource source;
	protected Vector3 mainCameraPosition;
	protected float MAX_POSITION;
	protected Vector3 CENTRE;

	private BlockBoss blockBoss;

	// [SerializeField]
	// private AudioClip cry;
	//
	// [SerializeField]
	// private AudioClip[] attackSounds;

	// void Awake() {
	// 	source = gameObject.GetComponent<AudioSource>();
	// 	source.clip = cry;
	// }

	protected virtual void Start() {
		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		enemyDamage = GetComponent<EnemyDamage>();
		animator.SetBool(TOXIC_ANIMATION, true);
		spriteRenderer = GetComponent<SpriteRenderer>();
		polygon_collider = GetComponent<PolygonCollider2D>();
		blockBoss = GameObject.FindObjectOfType<BlockBoss>();
		// circle_collider = GetComponent<CircleCollider2D>();
		// circle_collider.enabled = false;
		blockBoss.StartBattle += BattleBegin;
		mainCameraPosition = Camera.main.transform.position;
		MAX_POSITION = Camera.main.aspect*Camera.main.orthographicSize - 5f;

	}

	void Update() {
		if (toCenter) {
			animator.SetBool(WALK_ANIMATION, true);
			Vector3 newPos = new Vector3(Camera.main.transform.position.x, transform.position.y, 0);
			transform.position = Vector3.MoveTowards(transform.position, newPos, speed*Time.deltaTime);
		}

		if (Mathf.Abs(Camera.main.transform.position.x - transform.position.x) < 0.1f) {
			animator.SetBool(WALK_ANIMATION, false);
			toCenter = false;
		}
		if (moveToBorder) {
			animator.SetBool(WALK_ANIMATION, true);
			transform.position += new Vector3(movementX, 0f, 0f)*speed*Time.deltaTime;
		}
	}


	void StopMoveToBorder() {
		animator.SetBool(WALK_ANIMATION, false);
		movementX = -movementX;
		transform.localScale = new Vector3(-1f*transform.localScale.x, transform.localScale.y, 0f);
		isAttack = false;
	}


	private void BattleBegin() {
		Cry();
		StartCoroutine(BossAttack());
	}

	private void Cry() {
		// source.Play();
	}

	private IEnumerator BossAttack() {
		while (enemyDamage.toxic) {
			if (!isAttack && !moveToBorder) {
				isAttack = true;
				float prob = Random.Range(0f, 1f);
				Debug.Log(prob);
				if (prob < 0.33f) {
					toCenter = true;
					while (toCenter) {yield return null;}
					StartCoroutine(Attack1());
					while (moveToBorder) {yield return null;}
				}
				else if (0.33f <= prob && prob < 0.66f)
					StartCoroutine(Attack2());
				else
					StartCoroutine(Attack3());
			}
			yield return new WaitForSeconds(3f);
		}

		yield break;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Finish") && moveToBorder) {
			moveToBorder = false;
			StopMoveToBorder();
		}
	}

	public abstract IEnumerator Attack1();
	public abstract IEnumerator Attack2();
	public abstract IEnumerator Attack3();

}
