using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum AIStatsKind { idle,Active,Chaseing}

[RequireComponent(typeof(NavMeshAgent))]
  
public class DogExample : MonoBehaviour
{
     public float LookRadius = 10f;
    public float stopRadis = 3f;
    public float MaxSpeed = 3;
    public Transform target;
    private NavMeshAgent _Agent;
    public Animator DogAni;
    private AIStatsKind _AIStats;
     public bool isChaseing;
    private void OnDrawGizmos()
    {
 
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, LookRadius);
            Gizmos.color = Color.blue;
       
    }
    private void Awake()
    {
         _Agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_AIStats == AIStatsKind.idle )
        {
            float distance = Vector3.Distance(target.position, transform.position);
 
            if (distance <= LookRadius)
            {
                DogAni.SetBool("awake", true);
                Invoke("_SlowWalk",0.5f);
                _AIStats = AIStatsKind.Active;
               _Agent.speed = 0.5f;
            }

        }

        if (_AIStats == AIStatsKind.Chaseing)
        {
            _Agent.SetDestination(target.position);
            DogAni.SetFloat("speed", _Agent.velocity.magnitude);
            Debug.Log( _Agent.velocity.magnitude);
            if (true)
            {
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance <=stopRadis)
                {
                    // this is where you wolud kill the player 
                
                }
            }
        }
    }
 
    void _SlowWalk()
    {
         
        _Agent.SetDestination(target.position);
        DogAni.SetFloat("speed",0.5f);
        Invoke("_ChasePlayer", 3.0f);


    }

    void _ChasePlayer()
    {
        DogAni.SetFloat("speed",1); 
        _AIStats = AIStatsKind.Chaseing;
        _Agent.speed = MaxSpeed;

    }
}
