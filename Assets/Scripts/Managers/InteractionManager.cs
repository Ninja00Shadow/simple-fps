using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }
    
    public Weapon hoveredWeapon = null;
    public AmmoCrate hoveredAmmoCrate = null;
    
    public Door hoveredDoor = null;
    
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 5))
        {
            GameObject objectHit = hit.transform.gameObject;
            
            if (objectHit.GetComponent<Weapon>() && !objectHit.GetComponent<Weapon>().isActiveWeapon)
            {
                // Disable outline on previously hovered weapon
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
                
                hoveredWeapon = objectHit.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupWeapon(objectHit.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            
            if (objectHit.GetComponent<AmmoCrate>())
            {
                // Disable outline on previously hovered ammo crate
                if (hoveredAmmoCrate)
                {
                    hoveredAmmoCrate.GetComponent<Outline>().enabled = false;
                }
                
                hoveredAmmoCrate = objectHit.gameObject.GetComponent<AmmoCrate>();
                hoveredAmmoCrate.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoCrate);
                    Destroy(objectHit.gameObject);
                }
            }
            else
            {
                if (hoveredAmmoCrate)
                {
                    hoveredAmmoCrate.GetComponent<Outline>().enabled = false;
                }
            }
            
            if (objectHit.GetComponent<Door>())
            {
                // Disable outline on previously hovered door
                if (hoveredDoor)
                {
                    hoveredDoor.GetComponent<Outline>().enabled = false;
                }
                
                hoveredDoor = objectHit.gameObject.GetComponent<Door>();
                hoveredDoor.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hoveredDoor.OpenDoor();
                }
            }
            else
            {
                if (hoveredDoor)
                {
                    hoveredDoor.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
