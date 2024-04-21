using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour
{
    public List<ZombieSpawn> zombieSpawns;

    public List<Enemy> zombiesAlive;
    public List<Enemy> deadZombies;

    public TextMeshProUGUI enemiesLeftText;

    public static ZombieSpawnController Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        zombiesAlive = new List<Enemy>();

        SpawnZombies();
    }

    public void SpawnZombies()
    {
        foreach (ZombieSpawn zombieSpawn in zombieSpawns)
        {
            zombiesAlive.Add(zombieSpawn.SpawnZombie());
        }
    }

    public void ResetZombies()
    {
        foreach (Enemy zombie in zombiesAlive)
        {
            Destroy(zombie.gameObject);
        }

        foreach (Enemy zombie in deadZombies)
        {
            Destroy(zombie.gameObject);
        }

        zombiesAlive.Clear();

        SpawnZombies();
    }

    private void Update()
    {
        if (zombiesAlive.Count != 0)
        {
            foreach (Enemy zombie in zombiesAlive)
            {
                if (zombie.isDead)
                {
                    deadZombies.Add(zombie);
                    zombiesAlive.Remove(zombie);
                }
            }
        }
        else
        {
            ResetScene.Instance.WinGame();
        }


        enemiesLeftText.text = $"-Enemies:  {zombiesAlive.Count}/10";
    }
}