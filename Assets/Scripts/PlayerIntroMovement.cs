using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerIntroMovement : MonoBehaviour
{

    private Rigidbody2D rb2d;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private float jumpHight = 7.5f;
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private bool DisableMovement = false;

    [SerializeField] private GameObject wall;
    DestroyObject DestroyScript;
    [SerializeField] private GameObject beer;
    DestroyObject magicTrigger;
    [SerializeField] private GameObject continueText;    
    TextMeshProUGUI tmpGUI;
    [SerializeField] private GameObject hiddenEntrance;
    DestroyObject destroyHidden;
    


    bool CanExit = false;

    private int frameCount= 0; 


    private enum MovementState { idle, moving, jumping, falling, beer, stay }
    // Start is called before the first frame update
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        DestroyScript = wall.GetComponent<DestroyObject>();
        magicTrigger = beer.GetComponent<DestroyObject>();
        tmpGUI = continueText.GetComponent<TextMeshProUGUI>();
        destroyHidden = hiddenEntrance.GetComponent<DestroyObject>();
        // PlayerPrefs.SetInt("Checkpoint", 0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!DisableMovement){
            if (frameCount >= 6){

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
        frameCount = frameCount + 1;
        if(transform.position.y <= -20){
            StartCoroutine(reset());
        }
        if(CanExit){
            
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
                PlayerPrefs.SetInt("Checkpoint", 0);
                PlayerPrefs.SetInt("Level", 1);
                Application.Quit();
                UnityEngine.Debug.Log("Quitted");
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
        //Turn to cat
        if(collision.tag == "Beer"){
            DisableMovement = true;
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
            StartCoroutine(StopAnim());
        }
        //Touched a spike
        if(collision.tag == "Killy Thingy"){
            DisableMovement = true;
            rb2d.velocity = new Vector2(0f, 0f);
            Destroy(GetComponent<SpriteRenderer>());
            StartCoroutine(reset());
        }
        if(collision.tag == "Portal1_toVoid"){
            transform.position=new Vector2(44.25f, -9f);
        }
        if(collision.tag == "Portal1_fromVoid"){
            transform.position=new Vector2(42.5f, 1f);
        }
        if(collision.tag == "EndPillar"){
            tmpGUI.enabled = true;
            CanExit = true;
        }
        if(collision.tag == "Hidden"){
            destroyHidden.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        tmpGUI.enabled = false;
        CanExit = false;
    }

    //Fell in void
    

    IEnumerator StopAnim(){
        yield return new WaitForSeconds(2.5f);
        magicTrigger.enabled = true;
        MovementState state;

        state = MovementState.beer;
        anim.SetInteger("state", (int)state);
        yield return new WaitForSeconds(7.91f);
        DestroyScript.enabled = true;
        DisableMovement = false;
        PlayerPrefs.SetInt("Checkpoint", 1);
        UnityEngine.Debug.Log(PlayerPrefs.GetInt("Checkpoint"));
    }

    IEnumerator reset(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Foreward");
    }
}