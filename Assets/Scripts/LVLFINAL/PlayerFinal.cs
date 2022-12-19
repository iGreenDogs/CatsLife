using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerFinal : MonoBehaviour
{

    private enum MovementState { idle, moving, jumping, falling, b2h}
    private Animator anim;
    private SpriteRenderer sprite;
    private Rigidbody2D rb2d;
    private BoxCollider2D coll;

    [SerializeField] private float jumpHight = 7.5f;
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private bool DisableMovement = false;
    private int frame = 0;

    [SerializeField] private LayerMask jumpableGround;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        DisableMovement = true;
    }

    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
        frame = frame + 1;
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
        } else if (frame <= 100){
            rb2d.velocity = new Vector2(0, jumpHight);
        } else {
            MovementState state;

            state = MovementState.b2h;
            anim.SetInteger("state", (int)state);
            if (IsGrounded()){
                transform.position = new Vector2(-6, 25);
                DisableMovement = false;
            }

        }
        if(transform.position.y <= -20){
            StartCoroutine(reset());
        }
    }

    //Checks if player is touching ground
    private bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);   
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "MainMenu"){
            PlayerPrefs.SetInt("Checkpoint", 0);
            PlayerPrefs.SetInt("Level", 1);
            SceneManager.LoadScene(0);
        }
        if(collision.gameObject.name == "ExitGame"){
            PlayerPrefs.SetInt("Checkpoint", 0);
            PlayerPrefs.SetInt("Level", 1);
            Application.Quit();
        }
    }

    IEnumerator reset(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("FinalRoom");
    }
}
