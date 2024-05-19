using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int lives = 3;
    
    public GameObject bloodyScreen;
    
    public TextMeshProUGUI livesText;
    public GameObject gameOverScreen;
    public GameObject winScreen;

    public bool isDead;
    
    private Animator animator;
    
    private void Start()
    {
        livesText.text = $"Lives: {lives}";
        animator = GetComponentInChildren<Animator>();
    }
    
    public void TakeDamage()
    {
        lives -= 1;
        
        if (lives <= 0)
        {
            PlayerDead();
            isDead = true;
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeathSound);
        }
        else
        {
            StartCoroutine(BloodyScreenEffect());
            livesText.text = $"Lives: {lives}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurtSound);
        }
    }

    private void PlayerDead()
    {
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovementScript>().enabled = false;

        if (WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>())
        {
            WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>().gameObject.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        
        animator.SetBool("isDead", true);
        
        livesText.gameObject.SetActive(false);
        
        GetComponent<ScreenBlackout>().StartFade();
        StartCoroutine(ShowGameOverText());        
    }
    
    public void RespawnPlayer()
    {
        transform.position = new Vector3(0, 1.84f, -95.6f);
        
        GetComponent<MouseMovement>().enabled = true;
        GetComponent<PlayerMovementScript>().enabled = true;
        
        animator.SetBool("isDead", false);
        
        Cursor.lockState = CursorLockMode.Locked;
        
        livesText.gameObject.SetActive(true);
        
        lives = 3;
        livesText.text = $"Lives: {lives}";
        
        isDead = false;
        GetComponent<ScreenBlackout>().ResetFade();
        
        gameOverScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
    }

    public void WinGame()
    {
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovementScript>().enabled = false;

        if (WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>())
        {
            WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>().isActiveWeapon = false;
        }
        
        Cursor.lockState = CursorLockMode.None;
        
        livesText.gameObject.SetActive(false);
        
        // GetComponent<ScreenBlackout>().StartFade();
        winScreen.gameObject.SetActive(true);    
    }

    private IEnumerator ShowGameOverText()
    {
        yield return new WaitForSeconds(1f);
        gameOverScreen.gameObject.SetActive(true);

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }
        
        var image = bloodyScreen.GetComponentInChildren<Image>();
 
        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;
 
        float duration = 2f;
        float elapsedTime = 0f;
 
        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
 
            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
 
            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;
 
            yield return null; ; // Wait for the next frame.
        }
        
        if (bloodyScreen.activeInHierarchy == true)
        {
            bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Player collided with something!");
        if (other.gameObject.CompareTag("ZombieHand"))
        {
            if (!isDead)
            { 
                TakeDamage();
            }
        }
    }
}
