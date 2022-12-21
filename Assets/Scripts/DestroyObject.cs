using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    public GameObject self;
    
    void Start()
    {
        if (self != null){
            Destroy(self);
        }
    }
    private void Update() {
        if (self != null){
            Destroy(self);
        }
    }
}
