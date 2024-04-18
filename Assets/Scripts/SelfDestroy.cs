using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float timeUntilDestroy;
    
    private void Start()
    {
        StartCoroutine(DestroySelf(timeUntilDestroy));
    }
    
    private IEnumerator DestroySelf(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
