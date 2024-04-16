using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

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
}