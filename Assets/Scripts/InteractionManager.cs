using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }
    
    public Weapon hoveredWeapon = null;
    
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
        
        if (Physics.Raycast(ray, out hit, 100))
        {
            GameObject objectHit = hit.transform.gameObject;
            
            if (objectHit.GetComponent<Weapon>() && !objectHit.GetComponent<Weapon>().isActiveWeapon)
            {
                hoveredWeapon = objectHit.GetComponent<Weapon>();
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
                    // hoveredWeapon = null;
                }
            }
        }
    }
}
