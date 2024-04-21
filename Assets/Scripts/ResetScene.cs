using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScene : MonoBehaviour
{
    public GameObject AmmoCratePrefab;
    public GameObject M1911Prefab;
    public GameObject M4Prefab;
    
    public GameObject Player;
    
    public static ResetScene Instance { get; set; }
    
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
        ResetAmmoCrates();
        ResetWeapons();
    }

    public void ResetAmmoCrates()
    {
        Instantiate(AmmoCratePrefab, new Vector3(4.67f, 1.97f, -89.4f), Quaternion.identity);
        Instantiate(AmmoCratePrefab, new Vector3(4.2f, 1.97f, -89.4f), Quaternion.identity);
        Instantiate(AmmoCratePrefab, new Vector3(3.73f, 1.97f, -89.4f), Quaternion.identity);
    }
    
    public void ResetWeapons()
    {
        Instantiate(M1911Prefab, new Vector3(3.54f, 2.015f, -87.578f), Quaternion.Euler(180, 35, -90));
        Instantiate(M4Prefab, new Vector3(3.491f, 1.987f, -88.626f), Quaternion.Euler(180, 30, -90));
    }

    public void ResetPlayer()
    {
        Player playerScript = Player.GetComponent<Player>();
        playerScript.RespawnPlayer();
    }

    public void ResetWeaponManager()
    {
        WeaponManager.Instance.ResetWeapons();
    }

    public void WinGame()
    {
        Player.GetComponent<Player>().WinGame();
    }
    
    public void ResetEverything()
    {
        ResetAmmoCrates();
        ResetWeapons();
        
        ResetPlayer();
        
        ResetWeaponManager();
        
        ZombieSpawnController.Instance.ResetZombies();
    }
}
