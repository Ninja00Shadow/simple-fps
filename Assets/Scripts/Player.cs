using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int lives = 3;
    
    public void TakeDamage()
    {
        lives -= 1;
        
        if (lives <= 0)
        {
            print("Game Over!");
        }
        else
        {
            print("Player took damage!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Player collided with something!");
        if (other.gameObject.CompareTag("ZombieHand"))
        {
            TakeDamage();
        }
    }
}
