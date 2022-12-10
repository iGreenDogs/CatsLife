using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;
    private Rigidbody2D rb2d;
    private BoxCollider2D coll;


    [SerializeField] private bool DisableMovement = false;
    [SerializeField] private bool CanExit = false;

    [SerializeField] private float jumpHight = 7.5f;
    [SerializeField] private float moveSpeed = 7.5f;

    [SerializeField] private LayerMask jumpableGround;


    private enum MovementState { idle, moving, falljump }
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if (!DisableMovement){

                float dirX = Input.GetAxis("Horizontal");

                rb2d.velocity = new Vector2(moveSpeed * dirX, rb2d.velocity.y);

                if (Input.GetKeyDown("space") || Input.GetKeyDown("w") || Input.GetKeyDown("up"))
                {
                    if (IsGrounded())
                    {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHight);
                    }   
                }

                UpdateAnimations(dirX);
        }
        //Exit Level
        if(CanExit){
            
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
                PlayerPrefs.SetInt("Checkpoint", 0);
                PlayerPrefs.SetInt("Level", 2);
                SceneManager.LoadScene(2);
            }
        }
    }
    //Movement Animations
    private void UpdateAnimations(float dirX) 
    {
        MovementState state;


        if (dirX > 0f)
        {
            state = MovementState.moving;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.moving;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;

        }

        if (rb2d.velocity.y > 0.1f)
        {
            state = MovementState.falljump;
        } 
        else if (rb2d.velocity.y < -0.1f)
        {
            state = MovementState.falljump;
        }

        anim.SetInteger("state", (int)state);
    }

    //Checks if player is touching ground
    private bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);   
    }
}
