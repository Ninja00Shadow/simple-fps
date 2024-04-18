using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int lives = 3;
    
    public GameObject bloodyScreen;
    
    public TextMeshProUGUI livesText;
    public GameObject gameOverText;

    public bool isDead;
    
    private void Start()
    {
        livesText.text = $"Lives: {lives}";
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
        
        GetComponentInChildren<Animator>().enabled = true;
        
        livesText.gameObject.SetActive(false);
        
        GetComponent<ScreenBlackout>().StartFade();
        StartCoroutine(ShowGameOverText());        
    }

    private IEnumerator ShowGameOverText()
    {
        yield return new WaitForSeconds(1f);
        gameOverText.gameObject.SetActive(true);
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
