using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes GameObject Player: (currently makes them jumpers & movers)
// - Handles player input
// - Sets up players
// - Holds and distributes items/powerups
public class PlayerController : MonoBehaviour
{

    public float maxSpeed = 10.0f;
    public Transform playerModel;
    public float groundSmoothTime = 0.05f;
    public float airSmoothTime = 0.25f;
    public float jumpHeight = 3.5f;
    public Transform groundCheck;
    public Vector2 groundSize;
    public LayerMask whatIsGround;
    public bool canDoubleJump;
    public bool canWallJump;
    public float maxWallSlideSpeed = 10.0f;
    // TODO do we need wallstick time or does it feel good anyway?
    //public float wallStickTime = 0.25f;
    //private float timeToWallUnstick;
    // TODO maybe use distance and height instead of Vector2s for walljumps
    //public float wallJumpDistance = 6.0f;
    //public float horizontalWallJumpForce = 2500.0f;
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    public Vector2 wallCheckSize;
    public LayerMask whatIsWall;

    protected int playerId;

    private Rigidbody2D rb2d;
    private Animator anim;
    private float jumpVelocity;
    private float velXSmoothing;
    private bool facingRight;
    private bool grounded;
    private bool wallSliding;
    private int wallDirX;
    private bool doubleJump;
    private bool execJump;
    private bool execWallJump;

    protected void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingRight = true;

        jumpVelocity = Utilities.InitialJumpVelocity(jumpHeight);
    }

    void FixedUpdate()
    {
        Vector2 vel = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
        float move = Input.GetAxis("Horizontal_" + playerId);

        if (grounded)
        {
            vel.x = Mathf.SmoothDamp(vel.x, move * maxSpeed, ref velXSmoothing, groundSmoothTime);
        }
        else
        {
            vel.x = Mathf.SmoothDamp(vel.x, move * maxSpeed, ref velXSmoothing, airSmoothTime);
        }

        if (execJump)
        {
            execJump = false;
            vel.y = jumpVelocity;
        }
        else if (execWallJump)
        {
            execWallJump = false;

            if (Equals(move, 0))
            {
                vel.x = -wallDirX * wallJumpOff.x;
                vel.y = wallJumpOff.y;
            }
            else if (Equals(Mathf.Sign(move), Mathf.Sign(wallDirX)))
            {
                vel.x = -wallDirX * wallJumpClimb.x;
                vel.y = wallJumpClimb.y;
            }
            else
            {
                vel.x = -wallDirX * wallLeap.x;
                vel.y = wallLeap.y;
            }
        }

        grounded = Physics2D.OverlapBox(groundCheck.position, groundSize, 0.0f, whatIsGround);
        SetGrounded(grounded);

        if (wallCheckRight != null)
        {
            if (Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0.0f, whatIsWall))
            {
                SetWallSliding(true, 1);
            }
            else if (Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0.0f, whatIsWall))
            {
                SetWallSliding(true, -1);
            }
            else
            {
                SetWallSliding(false, 0);
            }
            if (wallSliding && !grounded && vel.y < 0)
            {
                if (vel.y < -maxWallSlideSpeed)
                {
                    vel.y = -maxWallSlideSpeed;
                }
            }
        }

        rb2d.velocity = vel;

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
        anim.SetFloat("HorizontalSpeed", Mathf.Abs(move));
        anim.SetFloat("VerticalSpeed", rb2d.velocity.y);
    }

    protected void Update()
    {
        // TODO weapon/item switching (return out of all other input)
        // TODO action event for switches/buttons
        if (Input.GetButtonDown("Jump_" + playerId))
        {
            if (grounded)
            {
                Jump();
            }
            else if (wallSliding && canWallJump)
            {
                WallJump();
            }
            else if (doubleJump && canDoubleJump)
            {
                Jump();
            }
        }
    }

    private void SetWallSliding(bool sliding, int direction)
    {
        wallSliding = sliding;
        wallDirX = direction;
        anim.SetBool("WallSliding", wallSliding);
        if (wallSliding && canWallJump)
        {
            doubleJump = true;
        }
    }

    private void SetGrounded(bool newGrounded)
    {
        grounded = newGrounded;
        anim.SetBool("Grounded", grounded);
        if (grounded)
        {
            doubleJump = true;
        }
    }

    private void WallJump()
    {
        execWallJump = true;

        anim.SetBool("WallSliding", false);
    }

    void Jump()
    {
        execJump = true;

        // makes sense because it can start animating right away and give visual feedback before the next physics step
        // although VerticalSpeed should be 0 still maybe the animation becomes wierd
        anim.SetBool("Grounded", false);

        if (doubleJump && !grounded)
        {
            doubleJump = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        // FIXME does not play well with bear
        // flip mechanism: http://answers.unity3d.com/answers/1060296/view.html
        playerModel.localEulerAngles = playerModel.eulerAngles + new Vector3(0.0f, 180.0f, -2 * playerModel.eulerAngles.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, (Vector3)groundSize);
        if (wallCheckRight && wallCheckLeft)
        {
            Gizmos.DrawCube(wallCheckRight.position, (Vector3)wallCheckSize);
            Gizmos.DrawCube(wallCheckLeft.position, (Vector3)wallCheckSize);
        }
    }
}
