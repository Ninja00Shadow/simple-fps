using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")] 
    public int totalPistolAmmo = 0;
    public int totalRifleAmmo = 0;
    public int totalShotgunAmo = 0;
    

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
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject slot in weaponSlots)
        {
            if (slot == activeWeaponSlot)
            {
                slot.SetActive(true);
            }
            else
            {
                slot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveWeapon(1);
        }
    }

    public void PickupWeapon(GameObject pickedWeapon)
    {
        AddWeaponToActiveSlot(pickedWeapon);
    }

    private void AddWeaponToActiveSlot(GameObject pickedWeapon)
    {
        DropCurrentWeapon(pickedWeapon);
        
        pickedWeapon.GetComponent<Collider>().enabled = false;

        pickedWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedWeapon.GetComponent<Weapon>();
        weapon.transform.localPosition = weapon.spawnPosition;
        weapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            GameObject currentWeapon = activeWeaponSlot.transform.GetChild(0).gameObject;

            currentWeapon.GetComponent<Weapon>().isActiveWeapon = false;
            currentWeapon.GetComponent<Weapon>().animator.enabled = false;

            currentWeapon.transform.SetParent(pickedWeapon.transform.parent);
            currentWeapon.transform.localPosition = pickedWeapon.transform.localPosition;
            currentWeapon.transform.localRotation = pickedWeapon.transform.localRotation;
            
            currentWeapon.gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    public void SwitchActiveWeapon(int slotIndex)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotIndex];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    public void PickupAmmo(AmmoCrate ammoCrate)
    {
        totalPistolAmmo += ammoCrate.pistolAmmo;
        totalRifleAmmo += ammoCrate.rifleAmmo;
        totalShotgunAmo += ammoCrate.shotgunAmmo;
    }

    public void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.WeaponModel.M1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.M4:
                totalRifleAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.Benelli_M4:
                totalShotgunAmo -= bulletsToDecrease;
                break;
        }
    }
    
    public void IncreaseTotalAmmo(int bulletsToIncrease, Weapon.WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.WeaponModel.M1911:
                totalPistolAmmo += bulletsToIncrease;
                break;
            case Weapon.WeaponModel.M4:
                totalRifleAmmo += bulletsToIncrease;
                break;
            case Weapon.WeaponModel.Benelli_M4:
                totalShotgunAmo += bulletsToIncrease;
                break;
        }
    }
    
    public int CheckAmmoLeftFor(Weapon.WeaponModel weaponModel)
    {
        return weaponModel switch
        {
            Weapon.WeaponModel.M1911 => totalPistolAmmo,
            Weapon.WeaponModel.M4 => totalRifleAmmo,
            Weapon.WeaponModel.Benelli_M4 => totalShotgunAmo,
            _ => 0
        };
    }

    public void ResetWeapons()
    {
        totalPistolAmmo = 0;
        totalRifleAmmo = 0;
        
        foreach (GameObject slot in weaponSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }
    
}