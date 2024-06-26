using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int hp = 100;
    private Animator _animator;
    
    private NavMeshAgent agent;
    
    public List<GameObject> hitBoxes;
    
    public bool isDead;
    
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
            
            isDead = true;
            
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDyingSound);
            
            // gameObject.GetComponent<Collider>().enabled = false;
            foreach (var hitBox in hitBoxes)
            {
                hitBox.GetComponent<Collider>().enabled = false;
            }
        }
        else
        {
            // _animator.SetTrigger("damage");
            
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHitSound);
            
            _animator.SetBool("isChasing", true);
        }
    }

    public void HeadShot(int damage)
    {
        hp -= damage * 3;
        
        Debug.Log("Headshot! HP: " + hp);
        
        if (hp <= 0)
        {
            int random = Random.Range(0, 2) + 1;
            _animator.SetTrigger("die" + random);
            
            isDead = true;
            
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDyingSound);

            foreach (var hitBox in hitBoxes)
            {
                hitBox.GetComponent<Collider>().enabled = false;
            }
            // gameObject.GetComponent<Collider>().enabled = false;
        }
        else
        {
            _animator.SetTrigger("damage");
            
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHitSound);
            
            _animator.SetBool("isChasing", true);
        }
    } 

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, 2.5f); // attack range
    //     
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(transform.position, 18f); // detection range
    //     
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(transform.position, 21f); // stop chase range
    // }
}
