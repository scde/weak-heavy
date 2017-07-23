using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

	public enum moveType{UseTransform, UsePhysics};
	public moveType moveTypes;
	public Transform[] pathPoints;
	public int currentPath = 0;
	public float reachDistance = 5.0f;
	public float speed = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		switch (moveTypes){
		case moveType.UseTransform:
			UseTransform ();
			break;
		case moveType.UsePhysics:
			UsePhysics();
			break;
		}
	}

	// Speed 0 - 1 and no Rigidbody!
	void UseTransform(){
		Vector3 dir = pathPoints [currentPath].position - transform.position;
		Vector3 dirNorm = dir.normalized;

		transform.Translate (dirNorm * speed);
		print ("transform: " + transform);
		print ("value: " + (dirNorm * speed));

		if (dir.magnitude <= reachDistance) {
			currentPath++;
			if (currentPath >= pathPoints.Length) {
				currentPath = 0;
			}
		}
	
	}

	// Speed > 200 and Rigidbody!
	void UsePhysics(){
		Vector3 dir = pathPoints [currentPath].position - transform.position;
		Vector3 dirNorm = dir.normalized;

		print ("X: " + (dirNorm.x * (speed * Time.fixedDeltaTime)));
		GetComponent<Rigidbody2D>().velocity = new Vector2 (dirNorm.x * (speed * Time.fixedDeltaTime), GetComponent<Rigidbody2D>().velocity.y);

		if (dir.magnitude <= reachDistance) {
			
			currentPath++;
			if (currentPath >= pathPoints.Length) {
				currentPath = 0;
			}
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

}
