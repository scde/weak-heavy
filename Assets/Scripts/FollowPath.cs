using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

	public enum moveType{UseTransform, UsePhysics};
	public moveType moveTypes;
	public GameObject Path;
	public int currentPath = 0;
	public float reachDistance = 2.0f;
	public float speed;
	public bool lookAtTarget;

	private Transform[] pathPoints;	
	private bool playerIsInRange = false;
	private GameObject playerInRange = null;
	private Transform ScissorTransform;                       

	// Use this for initialization
	void Start () {
		ScissorTransform = GetComponentInParent<Transform> ();
		findPathPoints ();
		if (lookAtTarget) {
			lookAtTarget (pathPoints [currentPath].transform);
		}
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
		Vector3 dir = playerInRange.transform.position - ScissorTransform.position;
		Vector3 dirNorm = dir.normalized;

		if (lookAtTarget) {
			lookAtTarget (playerInRange.transform);
		}

		switch (moveTypes) {
		case moveType.UseTransform:
			ScissorTransform.Translate (dirNorm * speed);
			break;
		case moveType.UsePhysics:
			GetComponentInParent<Rigidbody2D>().velocity = new Vector2 (dirNorm.x * (speed * Time.fixedDeltaTime), GetComponentInParent<Rigidbody2D>().velocity.y);
			break;
		}
	}

	// Speed Input:
	// * Transform: Speed 0 - 0.1 and no Rigidbody!
	// * Physics: Speed > 200 and Rigidbody! 
	// *

	void MoveToNextWaypoint(){
		Vector3 dir = pathPoints [currentPath].position - ScissorTransform.position;
		Vector3 dirNorm = dir.normalized;

		if (lookAtTarget) {
			lookAtTarget (pathPoints [currentPath].transform);
		}
			
		switch (moveTypes) {
		case moveType.UseTransform:
			ScissorTransform.Translate (dirNorm * speed);
			break;
		case moveType.UsePhysics:
			GetComponentInParent<Rigidbody2D>().velocity = new Vector2 (dirNorm.x * (speed * Time.fixedDeltaTime), GetComponentInParent<Rigidbody2D>().velocity.y);
			break;
		}

		if (dir.magnitude <= reachDistance) {
			currentPath++;
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
		
	void lookAtTarget(Transform target){
		Vector3 dir = target.position - ScissorTransform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.AngleAxis(angle -180, Vector3.forward);

		if (Mathf.Abs(angle) < 90) {
			transform.parent.transform.rotation = Quaternion.Euler (0, 0, 180);
		} else {
			transform.parent.transform.rotation = Quaternion.Euler (0, 0, 0);
		}
	}
}
