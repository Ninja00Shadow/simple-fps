using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            CreateBulletImpactEffect(other);
            
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            CreateBulletImpactEffect(other);
            
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Bottle"))
        {
            other.gameObject.GetComponent<Bottle>().Shatter();
        }

        if (other.gameObject.CompareTag("EnemyHead"))
        {
            if (!other.transform.parent.gameObject.GetComponent<Enemy>().isDead)
            {
                other.transform.parent.gameObject.GetComponent<Enemy>().HeadShot(damage);
            }
            
            CreateBloodSprayEffect(other);
            
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (!other.transform.parent.gameObject.GetComponent<Enemy>().isDead)
            {
                other.transform.parent.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            
            CreateBloodSprayEffect(other);
            
            Destroy(gameObject);
        }
    }

    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSplatterEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        
        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
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
