using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    public float moveSpeed = 1f;
    public bool faceLeft = false;

    public float jumpForce = 10f;
    public float groundCheckRange = 1f;
    private bool canJump;
    public bool onGround;

    private bool onLadder = false;
    public float climbSpeed = 3f;
    private float ladderSpeed;
    private bool ladderJump = false;
    public float ladderJumpTime = 1f;
    private float ladderJumpTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sprite = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();

        ladderJumpTimer = ladderJumpTime;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, (-Vector2.up*groundCheckRange));
    }

    private void Update()
    {
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, -Vector2.up, groundCheckRange, LayerMask.GetMask("Platform"));

        canJump = false;

        if (groundCheck.collider)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        if (onGround)
        {
            canJump = true;
        }

        if (onLadder)
        {
            canJump = true;
        }

        if (ladderJump)
        {
            if(ladderJumpTimer <= 0)
            {
                ladderJump = false;
                ladderJumpTimer = ladderJumpTime;
            }
            else
            {
                ladderJumpTimer -= Time.deltaTime;
                onLadder = false;
            }
        }

        //Animation
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }


        if(rb.velocity.y > 0.5 && !onGround)
        {
            anim.SetBool("isJumpingUp", true);
            anim.SetBool("isJumpingDown", false);
        }           
        else if(rb.velocity.y < -0.5 && !onGround)
        {
            anim.SetBool("isJumpingDown", true);
            anim.SetBool("isJumpingUp", false);
        }
        else if(onGround)
        {
            anim.SetBool("isJumpingUp", false);
            anim.SetBool("isJumpingDown", false);
        }

        if (onLadder)
        {
            anim.SetBool("isClimbing", true);
        }
        if(!onLadder)
        {
            anim.SetBool("isClimbing", false);

        }

        Debug.Log(rb.velocity.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Horizontal
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, rb.velocity.y);

        //Flip Sprite
        sprite.flipX = faceLeft;


        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            faceLeft = true;
        }
        else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            faceLeft = false;
        }

        //Jump
        if (Input.GetButtonDown("Jump") && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //CLimb
        if (onLadder)
        {
            
            if (!ladderJump)
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            
            ladderSpeed = Input.GetAxisRaw("Vertical") * climbSpeed;

            rb.velocity = new Vector2(rb.velocity.x, ladderSpeed);
        }
        else
        {
            rb.gravityScale = 1;
        }

        //ladder jump
        if(onLadder && Input.GetButtonDown("Jump"))
        {
            ladderJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            onLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            onLadder = false;
        }
    }
}
