using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public GameObject initialState;
    public List<Rigidbody> allParts = new List<Rigidbody>();

    public void Shatter()
    {
        Destroy(initialState);
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;
        }
    }
}
