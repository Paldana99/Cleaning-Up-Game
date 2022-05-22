using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public List<Transform> PathPoints;
    private int currentPathPoint = 0;
    public float distanceToChase = 100f;
    private bool chasing = false;

    private Transform target;
    public Transform player;
    public float speed = 200f;
    public float nextWaypointDistance = 0.5f;

    private Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    private float actualScale;

    Seeker seeker;
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidBody = GetComponent<Rigidbody2D>();

        target = PathPoints[currentPathPoint];

        actualScale = GetComponent<Transform>().localScale.y;

        InvokeRepeating("UpdatePath", 0f, .5f);

    }

    public void stopInvoke() {
        Debug.Log("Se cancela el UpdatePath");
        CancelInvoke();
    }

    void UpdatePath()
    {
        if (distanceToPlayer() < distanceToChase) {
            target = player;
            chasing = true;
            nextWaypointDistance = 2f;
        } else {
            target = PathPoints[currentPathPoint];
            chasing = false;
            nextWaypointDistance = 0.5f;
        }
        if (seeker.IsDone())
            seeker.StartPath(rigidBody.position, target.position, OnPathComplete);
    }

    float distanceToPlayer() {
        float distance = Vector3.Distance(player.position, rigidBody.position);
        return distance;
    }

    void OnPathComplete(Path p){
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) 
        {
            reachedEndOfPath = true;
            if (!chasing) {
                if (currentPathPoint == PathPoints.Count - 1) {
                    currentPathPoint = 0; 
                } else
                {
                    currentPathPoint++;
                }
                target = PathPoints[currentPathPoint];
                return;
            } else
            {
                return;
            }
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidBody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rigidBody.AddForce(force);

        float distance = Vector2.Distance(rigidBody.position, path.vectorPath[currentWaypoint]);

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
