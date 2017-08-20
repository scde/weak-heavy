﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

	public enum moveType{UseTransform, UsePhysics};
	public moveType moveTypes;
	public GameObject Path;
	public int currentPath = 0;
	public float reachDistance = 5.0f;
	public float speed = 5.0f;

	private Transform[] pathPoints;	
	public bool facingRight;
	private bool playerIsInRange = false;
	private GameObject playerInRange = null;

	// Use this for initialization
	void Start () {
		findPathPoints ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		//print (other.gameObject.name);
		//print (other.gameObject.tag);
		if (other.gameObject.tag == "Player") {
			playerIsInRange = true;
			playerInRange = other.gameObject;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		
		if (other.gameObject.tag == "Player") {
		playerIsInRange = true;
			playerInRange = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		playerIsInRange = false;
		playerInRange = null;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (playerIsInRange) {
			//print ("----------------------------------PLAYER IN RANGE!!!!-----------------------------------");
			moveToPlayer();
		} else {
			MoveToNextWaypoint ();
			//print ("-----------------------------------NOT IN RANGE!!!!-----------------------------------------");
		}

	}

	void moveToPlayer ()
	{
		Vector3 dir = playerInRange.transform.position - transform.position;
		Vector3 dirNorm = dir.normalized;

		/*Vector3 targetPostition = new Vector3( pathPoints[currentPath].position.x, 
			this.transform.position.y, 
			pathPoints[currentPath].position.z ) ;
		transform.LookAt (targetPostition);*/

		/*if (dir.x < 0 && facingRight) {
			Flip (dirNorm);
		} else if (dir.x > 0 && !facingRight) {
			Flip (dirNorm);
		} else {
			// do nothing
		}*/

		switch (moveTypes) {
		case moveType.UseTransform:
			transform.Translate (dirNorm * speed);
			break;
		case moveType.UsePhysics:
			GetComponent<Rigidbody2D>().velocity = new Vector2 (dirNorm.x * (speed * Time.fixedDeltaTime), GetComponent<Rigidbody2D>().velocity.y);
			break;
		}
	}

	// Speed Input:
	// * Transform: Speed 0 - 0.1 and no Rigidbody!
	// * Physics: Speed > 200 and Rigidbody! 
	// *

	void MoveToNextWaypoint(){
		Vector3 dir = pathPoints [currentPath].position - transform.position;
		Vector3 dirNorm = dir.normalized;

		/*if (dir.x < 0 && facingRight) {
			Flip (dirNorm);
		} else if (dir.x > 0 && !facingRight) {
			Flip (dirNorm);
		} else {
			// do nothing
		}*/


		switch (moveTypes) {
		case moveType.UseTransform:
			transform.Translate (dirNorm * speed);
			break;
		case moveType.UsePhysics:
			GetComponent<Rigidbody2D>().velocity = new Vector2 (dirNorm.x * (speed * Time.fixedDeltaTime), GetComponent<Rigidbody2D>().velocity.y);
			break;
		}

		if (dir.magnitude <= reachDistance) {
			currentPath++;
			/*Vector3 targetPostition = new Vector3( pathPoints[currentPath].position.x, 
				this.transform.position.y, 
				pathPoints[currentPath].position.z ) ;
			transform.LookAt (targetPostition);*/
			Flip (dirNorm);
			if (currentPath >= pathPoints.Length) {
				currentPath = 0;
			}
		}
	
	}

	void findPathPoints ()
	{
		int numChildObjects = Path.transform.childCount;
		pathPoints = new Transform[numChildObjects];

		for (int i = 0; i < numChildObjects; i++) {
			pathPoints [i] = Path.transform.GetChild (i);
		}
	}	
		

	void OnDrawGizmos (){
		if (pathPoints == null) {
			return;
		}
		foreach (Transform pathPoint in pathPoints) {
			if (pathPoint){
				Gizmos.DrawSphere (pathPoint.position, reachDistance);
			}
		}
	}


	private void Flip(Vector3 dirNorm) {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		if ((dirNorm.x < 0 && theScale.x > 0) || (dirNorm.x > 0 && theScale.x < 0)) {
			theScale.x *= -1;
		}
		transform.localScale = theScale;
	}

}
