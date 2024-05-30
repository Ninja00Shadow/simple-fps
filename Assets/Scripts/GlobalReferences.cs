using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set; }
    
    public GameObject bulletImpactEffectPrefab;
    
    public GameObject bloodSplatterEffectPrefab;
    
    public GameObject m1911Prefab;
    
    public GameObject m4Prefab;
    
    public GameObject benneliPrefab;
    
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
    
}
