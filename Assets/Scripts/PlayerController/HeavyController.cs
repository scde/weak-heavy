using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyController: PlayerController {

	public float runSpeed;
	public float walkSpeed;
	bool running;

	Rigidbody myRB;
	Animator myAnim;

	bool facingRight;

	bool grounded = false;
	Collider[] groundCollisions;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public float jumpHeigth;

	void Start () {
		myRB = GetComponent<Rigidbody>();
		myAnim = GetComponenty<Animator>();
		facingRight = true;
	}

	void Update () {
		
	}

	void FixedUpdate() {

		running = false;


		if (grounded && Input.GetAxis("Jump")>0) {
			grounded = false;
			myAnim.SetBool ("grounded", grounded);
			myRB.AddForce (new Vector3 (0, jumpHeigth, 0));
		}

		groundCollisions = Physics.OverLapSphere (groundCheck.position, groundCheckRadius, groundLayer);
		if (groundCollisions.Length > 0) {
			grounded = true;
		} else {
			grounded = false;
		}

		myAnim.SetBool ("grounded", grounded);

		float move = Intput.GetAxis("Horizontal");
		myAnim.SetFloat("Speed", Mathf.Abs(move));

		float sneaking = Input.GetAxisRaw ("Fire3");
		myAnim.SetFloat ("sneaking", sneaking);

		float shooting = Input.GetAxis ("Fire1");
		myAnim.SetFloat ("shooting", shooting);
		if ((sneaking > 0 || firing > 0) && grounded) {
			myRB.velocity = new Vector3 (move * walkSpeed, myRB.velocity.y, 0);
		} else {
			myRB.velocity = new Vector3(move * runSpeed, myRB.velocity.y, 0);
			if(Mathf.Abs(move) > 0) {
				running = true;
			}
		}

		
		if (move > 0 && !facingRight){
			Flip();
		} else if (move < 0 && facingRight) {
			Flip();
		}
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localeScale;
		theScale.z = -1;
		transform.localeScale = theScale;
	}

	public float GetFacing(){
		if (facingRight) {
			return 1;
		} else {
			return -1;
		}
	}

	public bool getRunning(){
		return (running);
	}
}
