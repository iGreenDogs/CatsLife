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
    [SerializeField] private GameObject continueText;   
    TextMeshProUGUI tmpGUI;

    [SerializeField] private LayerMask jumpableGround;

    private Vector2[] portalDumps = {
        new Vector2(-6,16.5f),
        new Vector2(-6.5f,1),
        new Vector2(18,-3.5f),
        new Vector2(24,-3.5f),
        new Vector2(30,-3.5f),
        new Vector2(36,-3.5f),
        new Vector2(18,0),
        new Vector2(24,0),
        new Vector2(30,0),
        new Vector2(36,0),
        new Vector2(18,4.5f),
        new Vector2(24,4.5f),
        new Vector2(30,4.5f),
        new Vector2(36,4.5f),
        new Vector2(18,8),
        new Vector2(24,8),
        new Vector2(30,8),
        new Vector2(36,8)
    };

    


    private enum MovementState { idle, moving, falljump }
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        tmpGUI = continueText.GetComponent<TextMeshProUGUI>();
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
        if(transform.position.y <= -20){
            StartCoroutine(reset());
        }
        //Exit Level
        if(CanExit){
            
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
                PlayerPrefs.SetInt("Checkpoint", 0);
                PlayerPrefs.SetInt("Level", 3);
                SceneManager.LoadScene(3);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LVL2_portal"){
            
            int randomIndex = Random.Range(0, portalDumps.Length);
            transform.position = portalDumps[randomIndex];
        }
        if(collision.tag == "EndPillar"){
            tmpGUI.enabled = true;
            CanExit = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        tmpGUI.enabled = false;
        CanExit = false;
    }

    IEnumerator reset(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("LVL2");
    }
}
