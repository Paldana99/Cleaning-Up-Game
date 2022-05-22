using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor;

public class EnemyAISpawned : MonoBehaviour
{
    private readonly string ENEMY_TAG = "Player";

    private Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 0.5f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool stop = false;
    private EnemyDamage enemyDamage;

    private float actualScale;

    private Seeker seeker;
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidBody = GetComponent<Rigidbody2D>();
        enemyDamage = GetComponent<EnemyDamage>();

        target = GameObject.FindGameObjectWithTag(ENEMY_TAG).GetComponent<Transform>();

        actualScale = GetComponent<Transform>().localScale.y;

        enemyDamage.OnEnemyDeath += StopPathFinding;

        InvokeRepeating(nameof(UpdatePath), 0f, .5f);

    }

    private void UpdatePath()
    {
        if (!target) return;
        seeker.StartPath(rigidBody.position, target.position, OnPathComplete);
    }


    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void StopPathFinding()
    {
        stop = true;
    }

    private void FixedUpdate()
    {
        if (stop == true) return;
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidBody.position).normalized;
        Vector2 force = direction * (speed * Time.deltaTime);

        rigidBody.AddForce(force);

        var distance = Vector2.Distance(rigidBody.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }


        actualScale = GetComponent<Transform>().localScale.y;
        if (rigidBody.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-actualScale, actualScale, 1f);
        } else if (rigidBody.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(actualScale, actualScale, 1f);
        }
    }
}
