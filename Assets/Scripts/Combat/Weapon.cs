using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int damage;

    [Header("Shooting")]
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;
    
    [Header("Shotgun")]
    public int pelletsPerShot = 8;

    [Header("Spread")]
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifetime = 3f;

    public GameObject muzzleFlash;

    internal Animator animator;

    [Header("Reload")]
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    [Header("Spawn Position")]
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    [Header("ADS")]
    public bool isADS;
    
    public enum WeaponModel
    {
        M1911,
        M4,
        Benelli_M4
    }

    public WeaponModel currentWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto,
        Shotgun
    }

    public ShootingMode currentShootingMode;

    public void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
        
        spreadIntensity = hipSpreadIntensity;
    }

    void Update()
    {
        if (isActiveWeapon)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
                foreach (Transform child2 in child)
                {
                    child2.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
                }
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }

            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }

            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagazineSoundM1911.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0); // Hold to shoot
            }
            else if (currentShootingMode is ShootingMode.Single or ShootingMode.Burst or ShootingMode.Shotgun)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0); // Click to shoot
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading &&
                WeaponManager.Instance.CheckAmmoLeftFor(currentWeaponModel) > 0)
            {
                Reload();
            }

            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0 &&
                WeaponManager.Instance.CheckAmmoLeftFor(currentWeaponModel) > 0)
            {
                Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
                foreach (Transform child2 in child)
                {
                    child2.gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }
        }
    }
    
    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.crosshair.SetActive(false);
                
        spreadIntensity = adsSpreadIntensity;
    }
    
    private void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.crosshair.SetActive(true);
                
        spreadIntensity = hipSpreadIntensity;
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleFlash.GetComponent<ParticleSystem>().Play();

        if (isADS)
        {
            animator.SetTrigger("recoilADS");
        }
        else
        {
            animator.SetTrigger("recoil");
        }

        SoundManager.Instance.PlayShootingSound(currentWeaponModel);

        readyToShoot = false;
        
        if (currentShootingMode != ShootingMode.Shotgun)
        {
            Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.damage = damage;
        
            bullet.transform.forward = shootingDirection;

            bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
            StartCoroutine(DestroyBullet(bullet, bulletPrefabLifetime));
        }
        else
        {
            for (int i = 0; i < pelletsPerShot; i++)
            {
                Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.damage = damage;
        
                bullet.transform.forward = shootingDirection;

                bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
                StartCoroutine(DestroyBullet(bullet, bulletPrefabLifetime));
            }
        }

        if (allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // we already fired one bullet
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(currentWeaponModel);

        animator.SetTrigger("reload");

        isReloading = true;
        Invoke(nameof(ReloadCompleted), reloadTime);
    }

    private void ReloadCompleted()
    {
        if (bulletsLeft > 0)
        {
            WeaponManager.Instance.IncreaseTotalAmmo(bulletsLeft, currentWeaponModel);
        }

        if (WeaponManager.Instance.CheckAmmoLeftFor(currentWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, currentWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(currentWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, currentWeaponModel);
        }

        isReloading = false;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        // Shooting from the middle of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; // Hit something
        }
        else
        {
            targetPoint = ray.GetPoint(100); // Hit nothing
        }

        // Vector3 direction = targetPoint - bulletSpawn.position;
        //
        // float zSpread = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        // float ySpread = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        //
        // return direction + new Vector3(0, ySpread, zSpread);
        Vector3 direction = targetPoint - bulletSpawn.position;

        float spreadX = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float spreadY = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        direction += bulletSpawn.right * spreadX + bulletSpawn.up * spreadY;

        return direction;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
    
    public string GetWeaponName()
    {
        return currentWeaponModel switch
        {
            WeaponModel.M1911 => "M1911",
            WeaponModel.M4 => "M4",
            WeaponModel.Benelli_M4 => "Benelli M4",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}