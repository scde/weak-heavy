using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyController : PlayerController
{

    private static HeavyController instance = null;

    public static HeavyController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // source: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
        // and https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
        // and https://stackoverflow.com/documentation/unity3d/2137/singletons-in-unity/14518/a-simple-singleton-monobehaviour-in-unity-c-sharp#t=201707311922517721043
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public float runSpeed;
    //public float walkSpeed;
    //bool running;

    //Rigidbody myRB;
    //Animator myAnim;

    //bool facingRight;

    //bool grounded = false;
    //Collider[] groundCollisions;
    //float groundCheckRadius = 0.2f;
    //public LayerMask groundLayer;
    //public Transform groundCheck;
    //public float jumpHeigth;

    //void Start () {
    //	myRB = GetComponent<Rigidbody>();
    //	myAnim = GetComponent<Animator>();
    //	facingRight = true;
    //}

    //void Update () {

    //}

    //void FixedUpdate() {

    //	running = false;


    //	if (grounded && Input.GetAxis("Jump")>0) {
    //		grounded = false;
    //		myAnim.SetBool ("grounded", grounded);
    //		myRB.AddForce (new Vector3 (0, jumpHeigth, 0));
    //	}

    //	groundCollisions = Physics.OverlapSphere (groundCheck.position, groundCheckRadius, groundLayer);
    //	if (groundCollisions.Length > 0) {
    //		grounded = true;
    //	} else {
    //		grounded = false;
    //	}

    //	myAnim.SetBool ("grounded", grounded);

    //	float move = Input.GetAxis("Horizontal");
    //	myAnim.SetFloat("Speed", Mathf.Abs(move));

    //	float sneaking = Input.GetAxisRaw ("Fire3");
    //	myAnim.SetFloat ("sneaking", sneaking);

    //	float shooting = Input.GetAxis ("Fire1");
    //	myAnim.SetFloat ("shooting", shooting);
    //	if ((sneaking > 0 || shooting > 0) && grounded) {
    //		myRB.velocity = new Vector3 (move * walkSpeed, myRB.velocity.y, 0);
    //	} else {
    //		myRB.velocity = new Vector3(move * runSpeed, myRB.velocity.y, 0);
    //		if(Mathf.Abs(move) > 0) {
    //			running = true;
    //		}
    //	}


    //	if (move > 0 && !facingRight){
    //		Flip();
    //	} else if (move < 0 && facingRight) {
    //		Flip();
    //	}
    //}

    //void Flip(){
    //	facingRight = !facingRight;
    //	Vector3 theScale = transform.localScale;
    //	theScale.z = -1;
    //	transform.localScale = theScale;
    //}

    //public float GetFacing(){
    //	if (facingRight) {
    //		return 1;
    //	} else {
    //		return -1;
    //	}
    //}

    //public bool getRunning(){
    //	return (running);
    //}
}
