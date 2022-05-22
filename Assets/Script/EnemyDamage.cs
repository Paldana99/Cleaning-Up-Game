using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float life;
    public HealthBarBoss healthBar;
    public float originalScale, final_scale , speed_scale;
    private bool first = true;
    public bool toxic = true;

    private Transform playerTransform;
    private Animator anim;
    private Rigidbody2D rb;
    private FlashDamage flashDamage;

    [SerializeField]
    private DamagePopup damagePopup;

    public event Action OnEnemyDeath;

    private void Start()
    {
      playerTransform =GetComponent<Transform>();
      anim = GetComponentInChildren<Animator>();
      rb = GetComponent<Rigidbody2D>();
      flashDamage = GetComponent<FlashDamage>();
      if (flashDamage == null) flashDamage = GetComponentInChildren<FlashDamage>();
    }

    public float GetLife()
    {
      return life;
    }

    public void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.CompareTag("PlayerAttack"))
      {
        GetDamage(other.gameObject.GetComponent<Bullet>().DAMAGE_MULTIPLIER);
      }
    }

    public void GetDamage(float damageRate)
    {
      flashDamage.Flash();
      life -= damageRate;
      damagePopup.Create(damagePopup, transform.position, (int)damageRate);
      if (life <= 0)
      {
        Debug.Log("Boss Death");
        toxic = false;
        if (OnEnemyDeath != null)
        {
          OnEnemyDeath();
        }
      }

    }

    public void Update()
    {
      if (!toxic && first) {
        InvokeRepeating(nameof(ToClean), 0f, .5f);
        first = false;
      }
    }

      public void ToClean() {

        if (GetComponent<ABoss>() != null)
          GetComponent<ABoss>().enabled = false;

        if (playerTransform.localScale.y > final_scale) {
          // Reduce scale until end
          var localScale = gameObject.transform.localScale;
          float sense =  localScale.x / Mathf.Abs(localScale.x);
          localScale -= new Vector3 (speed_scale * sense , speed_scale, 0 );
          gameObject.transform.localScale = localScale;
        } else {
          anim.SetBool("Toxic", false);
          anim.SetBool("Walk", false);
          gameObject.layer = 11;
          // Wait until stop moving
          // while (rb.velocity != Vector2.zero)
          // {
          //     continue;
          // }

          CancelInvoke();
        }
    }
}
