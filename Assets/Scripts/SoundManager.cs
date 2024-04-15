using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    
    public AudioSource shootingChannel;
    
    public AudioClip M1911ShootingSound;
    public AudioClip M4ShootingSound;
    
    public AudioSource reloadSoundM1911;
    public AudioSource reloadSoundM4;
    
    public AudioSource emptyMagazineSoundM1911;
    
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
    
    public void PlayShootingSound(Weapon.WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.WeaponModel.M1911:
                shootingChannel.PlayOneShot(M1911ShootingSound);
                break;
            case Weapon.WeaponModel.M4:
                shootingChannel.PlayOneShot(M4ShootingSound);
                break;
        }
    }
    
    // public void PlayEmptyMagazineSound(Weapon.WeaponModel weaponModel)
    // {
    //     switch (weaponModel)
    //     {
    //         case Weapon.WeaponModel.M1911:
    //             emptyMagazineSoundM1911.Play();
    //             break;
    //         case Weapon.WeaponModel.M4:
    //             break;
    //     }
    // }
    
    public void PlayReloadSound(Weapon.WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.WeaponModel.M1911:
                reloadSoundM1911.Play();
                break;
            case Weapon.WeaponModel.M4:
                reloadSoundM4.Play();
                break;
        }
    }
    
}
