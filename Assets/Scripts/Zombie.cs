using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int hp = 100;
    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        
        if (hp <= 0)
        {
            _animator.SetTrigger("die");
            Destroy(gameObject);
        }
        else
        {
            _animator.SetTrigger("damage");
        }
    }
}
