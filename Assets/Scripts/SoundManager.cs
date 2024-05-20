using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    
    [Header("Weapon Sounds")]
    public AudioSource shootingChannel;
    
    public AudioClip M1911ShootingSound;
    public AudioClip M4ShootingSound;
    public AudioClip BenneliM4ShootingSound;
    
    public AudioSource reloadSoundM1911;
    public AudioSource reloadSoundM4;
    public AudioSource benelliM4ReloadChannel;
    
    public AudioClip reloadSoundBenneliM4;
    
    public AudioSource emptyMagazineSoundM1911;
    
    [Header("Zombie Sounds")]
    public AudioSource zombieChannel1;
    public AudioSource zombieChannel2;
    
    public AudioClip zombieWalkingSound;
    public AudioClip zombieChasingSound;
    public AudioClip zombieAttackingSound;
    public AudioClip zombieHitSound;
    public AudioClip zombieDyingSound;
    
    [Header("Player Sounds")]
    public AudioSource playerChannel;
    public AudioClip playerHurtSound;
    public AudioClip playerDeathSound;
    
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
            case Weapon.WeaponModel.Benelli_M4:
                shootingChannel.PlayOneShot(BenneliM4ShootingSound);
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
            case Weapon.WeaponModel.Benelli_M4:
                StartCoroutine(PlayBenelliM4ReloadSound());
                break;
        }
    }

    private IEnumerator PlayBenelliM4ReloadSound()
    {
        for (int i = 0; i < 7; i++)
        {
            benelliM4ReloadChannel.PlayOneShot(reloadSoundBenneliM4);
            yield return new WaitForSeconds(0.4f);
        }
    }
}
