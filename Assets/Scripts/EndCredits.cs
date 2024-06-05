using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCredits : MonoBehaviour
{
    public GameObject endCreditsCanvas;
    
    private bool active = false;
    
    public Transform player;

    void Update()
    {
        if (active)
        {
            Vector3 direction = player.position - endCreditsCanvas.transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            endCreditsCanvas.transform.rotation = rotation * Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endCreditsCanvas.SetActive(true);
            active = true;
            StartCoroutine(EndOfCredits());
        }
    }
    
    private IEnumerator EndOfCredits()
    {
        yield return new WaitForSeconds(21f);
        Debug.Log("End of credits");
    }
}
