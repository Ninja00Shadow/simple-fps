using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

    [Header("Shooting")]
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

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
        M4
    }

    public WeaponModel currentWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
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
            else if (currentShootingMode is ShootingMode.Single or ShootingMode.Burst)
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

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBullet(bullet, bulletPrefabLifetime));

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

        Vector3 direction = targetPoint - bulletSpawn.position;

        float zSpread = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float ySpread = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, ySpread, zSpread);
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
}