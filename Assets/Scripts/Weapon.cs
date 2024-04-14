using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{    
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;
    
    // Burst fire
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;
    
    // Spread
    public float spreadIntensity;
    
    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifetime = 3f;
    
    public GameObject muzzleFlash;
    
    private Animator animator;
    private static readonly int Recoil = Animator.StringToHash("recoil");
    
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
    }

    void Update()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0); // Hold to shoot
        }
        else if (currentShootingMode is ShootingMode.Single or ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0); // Click to shoot
        }

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger(Recoil);
        
        SoundManager.Instance.shootingSoundM1911.Play();
        
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
        
        float xSpread = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float ySpread = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        
        return direction + new Vector3(xSpread, ySpread, 0);
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
