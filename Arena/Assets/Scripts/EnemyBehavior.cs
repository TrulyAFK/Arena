using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public Transform patrolRoute;
    public List<Transform> locations;

    private int loactionIndex=0;
    private NavMeshAgent agenet;

    private int _lives=3;
    public int EnemyLives
    {
        get{return _lives;}
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy down");
            }
        }
    }
    void Start()
    {
        agenet = GetComponent<NavMeshAgent>();
        player=GameObject.Find("Player").transform;
        InitializePatrolRoute();
        MoveToNextPatrolLocaiton();
    }
    void InitializePatrolRoute()
    {
     foreach(Transform child in patrolRoute)
        {
            locations.Add(child);
        }   
    }
    void Update()
    {
        if (agenet.remainingDistance < 0.2f && !agenet.pathPending)
        {
            MoveToNextPatrolLocaiton();
        }
    }
    void MoveToNextPatrolLocaiton()
    {
        if(locations.Count==0) 
            return;
        agenet.destination=locations[loactionIndex].position;
        loactionIndex=(loactionIndex+1)%locations.Count;
    }
    void OnTriggerStay(Collider other)
    {
        if(other.name == "Player")
        {
            agenet.destination=player.position;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            Debug.Log("Player Detected");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives-=1;
            Debug.Log("Crital hit!");
        }
    }
    
}
