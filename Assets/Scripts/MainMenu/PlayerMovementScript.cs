using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    private enum MovementState { idle, moving, falljump }
    private Animator anim;
    private SpriteRenderer sprite;
    private Rigidbody2D rb2d;
    private BoxCollider2D coll;

    [SerializeField] private float jumpHight = 7.5f;
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private bool DisableMovement = false;
    [SerializeField] private bool CanExit = false;

    [SerializeField] private LayerMask jumpableGround;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);   
    }

    void Update()
    {
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
        if(CanExit){
            
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
                PlayerPrefs.SetInt("Checkpoint", 0);
                PlayerPrefs.SetInt("Level", 3);
                SceneManager.LoadScene(3);
            }
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
            state = MovementState.falljump;
        } 
        else if (rb2d.velocity.y < -0.1f)
        {
            state = MovementState.falljump;
        }

        anim.SetInteger("state", (int)state);
    }
}
