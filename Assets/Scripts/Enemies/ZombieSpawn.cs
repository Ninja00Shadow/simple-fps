using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    public GameObject zombiePrefab;
    
    public Enemy SpawnZombie()
    {
        GameObject zombie = Instantiate(zombiePrefab, transform.position, Quaternion.identity);
        return zombie.GetComponent<Enemy>();
    }
}
