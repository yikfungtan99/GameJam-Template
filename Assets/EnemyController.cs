using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    private AIPath aiPath;
    private Rigidbody2D rb;

    public Transform target;
    public float refreshTime = 0.5f;

    public float speed = 1f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;

    private vision vision;

    private void Start()
    {
        aiPath = this.GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        vision = this.GetComponentInChildren<vision>();

        rb = this.GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, refreshTime);

        seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0; 
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        if (vision.detectEnemy)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 move = direction * speed * Time.deltaTime;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            rb.AddForce(move, ForceMode2D.Impulse);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }

        if(rb.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }else if(rb.velocity.x <= -0.01f){
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
