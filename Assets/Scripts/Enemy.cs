using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int hp = 100;
    private Animator _animator;
    
    private NavMeshAgent agent;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        
        if (hp <= 0)
        {
            int random = Random.Range(0, 2) + 1;
            _animator.SetTrigger("die" + random);
        }
        else
        {
            _animator.SetTrigger("damage");
        }
    }
    
    private void Update()
    {
        if (agent.remainingDistance > 0.1f)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }
}
