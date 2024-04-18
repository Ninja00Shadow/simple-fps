using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{
    private float timer;
    public float patrolingTime = 0f;
    
    private Transform player;
    private NavMeshAgent agent;
    
    public float detectionRange = 18f;
    public float patrolSpeed = 2f;
    
    List<Transform> waypoints = new List<Transform>();
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        
        agent.speed = patrolSpeed;
        timer = 0f;
        
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in waypointCluster.transform)
        {
            waypoints.Add(t);
        }
        
        Vector3 newDestination = waypoints[Random.Range(0, waypoints.Count)].position;
        agent.SetDestination(newDestination);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel1.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel1.clip = SoundManager.Instance.zombieWalkingSound;
            SoundManager.Instance.zombieChannel1.PlayDelayed(1f);
        }
        
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer >= patrolingTime)
        {
            animator.SetBool("isPatroling", false);
        }
        
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer <= detectionRange)
        {
            animator.SetBool("isChasing", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        
        SoundManager.Instance.zombieChannel1.Stop();
    }
}
