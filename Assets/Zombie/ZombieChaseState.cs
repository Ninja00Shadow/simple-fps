using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    
    public float chaseSpeed = 6f;
    
    public float stopChaseRange = 21f;
    public float attackRange = 2.5f;
    public float searchDuration = 10f;
    private float searchTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        
        agent.speed = chaseSpeed;
        searchTimer = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel1.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel1.PlayOneShot(SoundManager.Instance.zombieChasingSound);
        }
        
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);
        
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer > stopChaseRange)
        {
            searchTimer += Time.deltaTime;
            if (searchTimer >= searchDuration)
            {
                animator.SetBool("isChasing", false);
            }
        }
        else
        {
            searchTimer = 0f;
        }

        if (distanceFromPlayer <= attackRange)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        
        SoundManager.Instance.zombieChannel1.Stop();
    }
}

