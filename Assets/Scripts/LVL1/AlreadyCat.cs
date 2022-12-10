using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlreadyCat : MonoBehaviour
{
    private int checkInt;
    private enum MovementState { idle, moving, jumping, falling, beer, stay };

    private Animator anim;

    [SerializeField] private GameObject wall;
    DestroyObject DestroyScript;
    [SerializeField] private GameObject beer;
    DestroyObject magicTrigger;
    private int frameCount= 0; 

    void Start()
    {
        checkInt = PlayerPrefs.GetInt("Checkpoint");
        UnityEngine.Debug.Log(PlayerPrefs.GetInt("Checkpoint"));
        anim = GetComponent<Animator>();
        if (checkInt >= 1){
            //change player to cat
            MovementState state;

            state = MovementState.beer;
            anim.SetInteger("state", (int)state);
            state = MovementState.idle;
            anim.SetInteger("state", (int)state);
            //destroy beer and wall
            DestroyScript = wall.GetComponent<DestroyObject>();
            magicTrigger = beer.GetComponent<DestroyObject>();
            DestroyScript.enabled = true;
            magicTrigger.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovementState state;

        if (checkInt >= 1){
            if (frameCount == 2){
                state = MovementState.beer;
                anim.SetInteger("state", (int)state);
            }
            if (frameCount == 4) {
                state = MovementState.idle;
                anim.SetInteger("state", (int)state);
            }
        }
        frameCount = frameCount + 1;
    }
}
