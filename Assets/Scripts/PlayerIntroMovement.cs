using System.Runtime.CompilerServices;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntroMovement : MonoBehaviour
{

    private Rigidbody2D rb2d;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private float jumpHight = 7.5f;
    [SerializeField] private float moveSpeed = 7.5f;

    [SerializeField] private bool DrankBeer = false;

    private enum MovementState { idle, moving, jumping, falling, beer, stay }
    // Start is called before the first frame update
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!DrankBeer){

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
       
    }

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
            state = MovementState.jumping;
        } 
        else if (rb2d.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);   
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Beer"){
            DrankBeer = true;
            rb2d.velocity = new Vector2(0, 0);
            StartCoroutine(StopAnim());
        }
    }

    IEnumerator StopAnim(){
        yield return new WaitForSeconds(1f);
        MovementState state;

        state = MovementState.beer;
        anim.SetInteger("state", (int)state);
        yield return new WaitForSeconds(7.91f);
        state = MovementState.stay;
        anim.SetInteger("state", (int)state);
    }
}
