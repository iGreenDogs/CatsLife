using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    public GameObject self;
    
    void Start()
    {
        Destroy(self);
    }
}
