using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class MainAttackMelee : MonoBehaviour
{

    private Animator animator;
    public int combo;
    [SerializeReference]
    private bool isAttack;

    public Transform attackPointL;
    public Transform attackPointR;
    public float rangeAttack = 0.5f;
    public LayerMask enemyLayers;
    public Material materialAttack;
    public Material materialNormal;

    // private MainVar mainVar;

    private SpriteRenderer playerSprite;

    [SerializeField] private float damageMelee = 5;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerSprite = GetComponentInParent<SpriteRenderer>();
        // mainVar = GameObject.FindObjectOfType<MainVar>();

    }

    public void Combos()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isAttack)
        // if (Input.GetKeyDown(KeyCode.X) && !isAttack && mainVar.isArmed)
        {
            isAttack = true;
            // TODO Enabled/Disable the Pointattack L or R
            if (!playerSprite.flipX){}
            GetComponent<SpriteRenderer>().material = materialAttack;
            Attack();
            animator.SetTrigger("Attack"+combo);
        }
    }

    private void Attack()
    {
        Collider2D[] hitEnemies;
        if (!playerSprite.flipX)
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPointR.position, rangeAttack, enemyLayers);
        } else
        {
            hitEnemies = Physics2D.OverlapCircleAll(attackPointL.position, rangeAttack, enemyLayers);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            var enemyDamage = enemy.GetComponent<EnemyDamage>();
            if (enemyDamage) enemy.GetComponent<EnemyDamage>().GetDamage(damageMelee);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPointR.position, rangeAttack);
        Gizmos.DrawWireSphere(attackPointL.position, rangeAttack);
    }

    public void Start_Combo()
    {
        isAttack = false;
        GetComponent<SpriteRenderer>().material = materialNormal;
        if (combo < 3)
        {
            combo++;

        }
    }

    public void Finish_Ani()
    {
        isAttack = false;
        GetComponent<SpriteRenderer>().material = materialNormal;
        combo = 0;
    }

}
