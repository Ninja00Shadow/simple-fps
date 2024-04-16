using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }
    
    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoText;
    public TextMeshProUGUI totalAmmoText;
    public Image ammoTypeImage;
    
    [Header("Weapon")]
    public Image activeWeaponImage;
    public Image secondaryWeaponImage;
    
    [Header("Throwables")]
    public Image lethalImage;
    public TextMeshProUGUI lethalAmountText;
    
    public Image tacticalImage;
    public TextMeshProUGUI tacticalAmountText;

    public Sprite emptySlotSprite;
    
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

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon secondaryWeapon = GetSecondaryWeaponSlot().GetComponentInChildren<Weapon>();
        
        if (activeWeapon)
        {
            magazineAmmoText.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoText.text = WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.currentWeaponModel).ToString();
            
            Weapon.WeaponModel weaponModel = activeWeapon.currentWeaponModel;
            ammoTypeImage.sprite = GetAmmoSpite(weaponModel);
            
            activeWeaponImage.sprite = GetWeaponSprite(weaponModel);

            if (secondaryWeapon)
            {
                secondaryWeaponImage.sprite = GetWeaponSprite(secondaryWeapon.currentWeaponModel);
            }
        }
        else
        {
            magazineAmmoText.text = "";
            totalAmmoText.text = "";
            
            ammoTypeImage.sprite = emptySlotSprite;
            activeWeaponImage.sprite = emptySlotSprite;
            secondaryWeaponImage.sprite = emptySlotSprite;
        }
    }

    private GameObject GetSecondaryWeaponSlot()
    {
        foreach (GameObject slot in WeaponManager.Instance.weaponSlots)
        {
            if (slot != WeaponManager.Instance.activeWeaponSlot)
            {
                return slot;
            }
        }

        return null;
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                return Instantiate(Resources.Load<GameObject>("M1911_Weapon").GetComponent<SpriteRenderer>().sprite);
            case Weapon.WeaponModel.M4:
                return Instantiate(Resources.Load<GameObject>("M4_Weapon").GetComponent<SpriteRenderer>().sprite);
            default:
                return null;
        }
    }

    private Sprite GetAmmoSpite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                return Instantiate(Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite);
            case Weapon.WeaponModel.M4:
                return Instantiate(Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite);
            default:
                return null;
        }
    }
}
