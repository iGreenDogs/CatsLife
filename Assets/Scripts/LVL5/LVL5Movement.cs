using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class LVL5Movement : MonoBehaviour
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

    [SerializeField] private GameObject continueText;    
    TextMeshProUGUI tmpGUI;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        tmpGUI = continueText.GetComponent<TextMeshProUGUI>();
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
                PlayerPrefs.SetInt("Level", 6);
                SceneManager.LoadScene(6);
            }
        }
    }

    private bool IsGrounded() {
        Debug.Log(Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround));
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
            state = MovementState.falljump;
        } 
        else if (rb2d.velocity.y < -0.1f)
        {
            state = MovementState.falljump;
        }

        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Killy Thingy"){
            DisableMovement = true;
            rb2d.velocity = new Vector2(0f, 0f);
            Destroy(GetComponent<SpriteRenderer>());
            StartCoroutine(reset());
        }
        /*if(collision.tag == "Hidden"){
            destroyHidden.enabled = true;
        }*/
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
        SceneManager.LoadScene("LVL5");
    }
}
