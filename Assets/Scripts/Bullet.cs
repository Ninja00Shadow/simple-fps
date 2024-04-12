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
            
            CreateBulletImpactEffect(other);
            
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            print("hit wall");
            
            CreateBulletImpactEffect(other);
            
            Destroy(gameObject);
        }
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );
        
        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
