using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int player = 0;
    public float maxSpeed = 10f;
    public float jumpForce = 500f;
    public bool canDoubleJump = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    private Rigidbody2D rb2d;
    private bool facingRight = true;
    private bool grounded = false;
    private float groundRadius = 0.2f;
    private bool doubleJump = false;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        if (grounded) {
            doubleJump = false;
        }

        float move = Input.GetAxis("Horizontal_P" + (player + 1));

        rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);
        //rb2d.AddForce(new Vector2(move * maxSpeed, rb2d.velocity.y));

        // Flips character (sprites) into moving direction
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }

    // Update is called once per frame
    void Update () {
        if ((grounded || (!doubleJump && canDoubleJump))  && Input.GetButtonDown("Jump_P" + (player + 1))) {
            //grounded = false;
            rb2d.AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !grounded) {
                doubleJump = true;
            }
        }
    }

    void Flip () {
        facingRight = !facingRight;
        Vector3 currentLocalScale = transform.localScale;

        currentLocalScale.x *= -1;
        transform.localScale = currentLocalScale;
    }
}
