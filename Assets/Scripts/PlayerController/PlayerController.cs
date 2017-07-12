using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int playerId = 0;
    public float maxSpeed = 10f;
    // TODO jumpHeight which translates to jumpForce (tutorial)
    public float jumpForce = 500f;
    public float horizontalWallJumpForce = 1000f;
    public bool canDoubleJump = false;
    public bool canWallJump = false;
    public Transform groundCheck;
	public Vector2 groundSize = new Vector2(1.25f, 0.65f);
	public Transform wallCheck;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    private Rigidbody2D rb2d;
    private Animator anim;
    private bool facingRight = true;
    private bool grounded = false;
    private bool onWall = false;
    //private float groundRadius = 0.2f;
    //private float wallRadius = 0.5f;
    private bool doubleJump = false;
    private bool execJump = false;



    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

	// FixedUpdate is called once per physics timestep
    void FixedUpdate () {
		float move = Input.GetAxis("Horizontal_" + playerId);
		anim.SetFloat("HorizontalSpeed", Mathf.Abs(move));
		rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);
		//rb2d.AddForce(new Vector2(move * maxSpeed, rb2d.velocity.y));

		if (execJump) {
            execJump = false;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f);
			rb2d.AddForce(new Vector2(0.0f, jumpForce));
		}
		anim.SetFloat("VerticalSpeed", rb2d.velocity.y);

//        if (Mathf.Abs(rb2d.velocity.x) > 0.0f) {
//			//print(rb2d.velocity.x);
//		}

		grounded = Physics2D.OverlapBox(groundCheck.position, groundSize, 0.0f, whatIsGround, 0.0f);
		SetGrounded(grounded);
		//grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		//anim.SetBool("Grounded", grounded);
		//onWall = Physics2D.OverlapCircle(wallCheck.position, wallRadius, whatIsWall);
		//if (grounded) {
		//    doubleJump = true;
		//}
		//if (onWall) {
		//    grounded = false;
		//    doubleJump = true;
		//}

        // Flips character (whole GameObject, not only sprite) into moving direction
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }

    // Update is called once per frame
    void Update () {
		if ((grounded || (doubleJump && canDoubleJump)) && Input.GetButtonDown("Jump_" + playerId)) {
    //        if (doubleJump)
				//print ("Doublejump!");
    //        if (grounded)
				//print ("Jump!");

			execJump = true;

			// makes sense because it can start animating right away and give visual feedback before the next physics step
			// although VerticalSpeed should be 0 still maybe the animation becomes wierd
			anim.SetBool("Grounded", false);

            if (doubleJump && !grounded) {
                doubleJump = false;
                //print("doubleJump = false;"); 
            }
        }
		if (canWallJump && onWall && Input.GetButtonDown("Jump_" + playerId)) {
			print ("Walljump!");
            WallJump();
        }
    }

 //   private void OnCollisionEnter2D(Collision2D col) {
	//	// LayerMask-Check: http://answers.unity3d.com/questions/50279/check-if-layer-is-in-layermask.html
	//	if (whatIsGround == (whatIsGround | (1 << col.gameObject.layer)))
 //           SetGrounded(true);
 //   }

	//private void OnCollisionStay2D(Collision2D col)
	//{
	//	//print("Stay");
	//	if (whatIsGround == (whatIsGround | (1 << col.gameObject.layer)))
	//		SetGrounded(true);
	//}

	//private void OnCollisionExit2D(Collision2D col)
	//{
	//	if (whatIsGround == (whatIsGround | (1 << col.gameObject.layer)))
	//		SetGrounded(false);
	//}

	//   private void OnTriggerEnter2D(Collider2D col) {
	//       if (col.gameObject.tag == "Ground")
	//           SetGrounded(true);
	//   }

	//private void OnTriggerStay2D(Collider2D col)
	//{
	//	if (col.gameObject.tag == "Ground")
	//		SetGrounded(true);
	//}

	//private void OnTriggerExit2D(Collider2D col)
	//{
	//       if (col.gameObject.tag == "Ground")
	//           SetGrounded(false);
	//}

	public void SetGrounded(bool newGrounded) {
        grounded = newGrounded;
        anim.SetBool("Grounded", grounded);
        if (grounded) {
			doubleJump = true;
			//print("doubleJump = true;");
		}
	}

    private void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
