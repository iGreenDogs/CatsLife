using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBeerTrigger : MonoBehaviour
{

    public GameObject self;
    private void OnTriggerEnter2D(Collider2D collision){
            Destroy(self);
    }
}
