using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }
    
    [Header("Player Data")]
    public bool wasSaved;
    public int playerHealth;
    public string firstWeapon;
    public string secondWeapon;
    public int activeWeaponSlot;
    public int pistolAmmo;
    public int rifleAmmo;
    public int shotgunAmmo;
    
    private void Awake()
    {
        Debug.Log("SaveLoadManager initialized");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(this);
    }

    private void SavePlayerData()
    {
        wasSaved = true;
        
        playerHealth = FindObjectOfType<Player>().health;
        
        activeWeaponSlot = WeaponManager.Instance.ActiveWeaponIndex();

        if (WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>())
        {
            firstWeapon = WeaponManager.Instance.weaponSlots[0].GetComponentInChildren<Weapon>().GetWeaponName();
        }
        else
        {
            firstWeapon = null;
        }

        if (WeaponManager.Instance.weaponSlots[1].GetComponentInChildren<Weapon>())
        {
            secondWeapon = WeaponManager.Instance.weaponSlots[1].GetComponentInChildren<Weapon>().GetWeaponName();
        }
        else
        {
            secondWeapon = null;
        }
        
        Debug.Log("First weapon: " + firstWeapon);
        Debug.Log("Second weapon: " + secondWeapon);
        Debug.Log("Active weapon slot: " + activeWeaponSlot);
        
        pistolAmmo = WeaponManager.Instance.totalPistolAmmo;
        rifleAmmo = WeaponManager.Instance.totalRifleAmmo;
        shotgunAmmo = WeaponManager.Instance.totalShotgunAmo;
        
        Debug.Log("Player data saved");
    }
    
    private void LoadPlayerData()
    {
        FindObjectOfType<Player>().health = playerHealth;
        
        Debug.Log("First weapon: " + firstWeapon);
        Debug.Log("Second weapon: " + secondWeapon);
        Debug.Log("Active weapon slot: " + activeWeaponSlot);

        WeaponManager.Instance.SwitchActiveWeapon(0);
        if (firstWeapon != null)
        {
            WeaponManager.Instance.PickupWeapon(GetWeaponModel(firstWeapon));
        }
        WeaponManager.Instance.SwitchActiveWeapon(1);
        if (secondWeapon != null)
        {
            WeaponManager.Instance.PickupWeapon(GetWeaponModel(secondWeapon));
        }
        
        WeaponManager.Instance.SwitchActiveWeapon(activeWeaponSlot);
        
        WeaponManager.Instance.totalPistolAmmo = pistolAmmo;
        WeaponManager.Instance.totalRifleAmmo = rifleAmmo;
        WeaponManager.Instance.totalShotgunAmo = shotgunAmmo;
        
        Debug.Log("Player data loaded");
    }

    private GameObject GetWeaponModel(string weaponName)
    {
        return weaponName switch
        {
            "M1911" => Instantiate(GlobalReferences.Instance.m1911Prefab),
            "M4" => Instantiate(GlobalReferences.Instance.m4Prefab),
            "Benelli M4" => Instantiate(GlobalReferences.Instance.benneliPrefab),
            _ => null
        };
    }
    
    public void SaveMapData()
    {
        SavePlayerData();
    }
    
    public void LoadMapData()
    {
        LoadPlayerData();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (wasSaved)
        {
            StartCoroutine(new WaitForSecondsRealtime(0.2f));
            LoadMapData();
        }
    }
}
