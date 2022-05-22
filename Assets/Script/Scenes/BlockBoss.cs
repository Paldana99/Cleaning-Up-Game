using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockBoss : MonoBehaviour
{
    private const string CmName = "CM vcam1";
    private const string PlayerTag = "Player";
    private GameObject camera;
    [SerializeField]
    private GameObject player;
    private BoxCollider2D[] childCollider;
    private EnemyDamage boss;
    private ParticleSystem[] particle;
    private Collider2D col;

    public event Action StartBattle;
    
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        camera = GameObject.Find(CmName);
        // player = GameObject.FindWithTag(PlayerTag);
        particle = GetComponentsInChildren<ParticleSystem>();
        boss = GameObject.FindObjectOfType<EnemyDamage>();
        boss.OnEnemyDeath += Unlock;
        childCollider = gameObject.GetComponentsInChildren<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag(PlayerTag)) return;
        if (boss.GetLife() > 0)
        {
            Block();
            if (StartBattle != null) StartBattle();
        }
            
    }

    private void Block()
    {
        Debug.Log("Lock!");
        camera.GetComponent<CinemachineVirtualCamera>().Follow = null;
        boss.healthBar.gameObject.SetActive(true);
        boss.healthBar.SetMaxHealth(boss.life);
        foreach (var collider in childCollider)
        {
            collider.enabled = true;
        }

        foreach (var par in particle)
        {
            par.Play();
        }
        col.enabled = false;
    }

    public void Unlock()
    {
        // Attached to event in EnemyDamage -> OnEnemyDeath by Boss.
        Debug.Log("Unlock!");
        camera.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        boss.healthBar.gameObject.SetActive(false);
        foreach (var collider in childCollider)
        {
            collider.enabled = false;
        }
        foreach (var par in particle)
        {
            par.Stop();
        }
    }
}
