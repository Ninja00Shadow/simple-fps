using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            print("hit " + other.gameObject.name);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            print("hit wall");
            Destroy(gameObject);
        }
    }
}
