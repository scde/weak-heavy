using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int player = 0;
    public float maxSpeed = 10f;
    public float jumpForce = 500f;
    public float horizontalWallJumpForce = 1000f;
    public bool canDoubleJump = false;
    public bool canWallJump = false;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer rend;
    private bool facingRight = true;
    private bool grounded = false;
    private bool onWall = false;
    private float groundRadius = 0.2f;
    private float wallRadius = 0.5f;
    private bool doubleJump = false;



    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

	// FixedUpdate is called once per pyhsics timestep
    void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("Grounded", grounded);
		onWall = Physics2D.OverlapCircle(wallCheck.position, wallRadius, whatIsWall);
        if (grounded) {
            doubleJump = false;
        }
        if (onWall) {
            grounded = false;
            doubleJump = false;
        }

        float move = Input.GetAxis("Horizontal_P" + (player + 1));
		anim.SetFloat("HorizontalSpeed", Mathf.Abs(move));

		rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);
        //rb2d.AddForce(new Vector2(move * maxSpeed, rb2d.velocity.y));

        // Flips character (sprites) into moving direction
        if (move > 0 && !facingRight) {
            facingRight = !facingRight;
			rend.flipX = false;
		}
        else if (move < 0 && facingRight) {
			facingRight = !facingRight;
			rend.flipX = true;
		}
    }

    // Update is called once per frame
    void Update () {
        if ((grounded || (!doubleJump && canDoubleJump)) && Input.GetButtonDown("Jump_P" + (player + 1))) {
			if (!grounded) {
				print ("Doublejump!");
			} else {
				print ("Jump!");
			}
            rb2d.AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !grounded) {
                doubleJump = true;
            }
        }
        if (canWallJump && onWall && Input.GetButtonDown("Jump_P" + (player + 1))) {
			print ("Walljump!");
            WallJump();
        }
    }

    private void WallJump() {
        //rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(0,jumpForce));
        //if (facingRight)
            //rb2d.velocity = new Vector2(maxSpeed * -1, rb2d.velocity.y);
        //else
            //rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
    }
}
