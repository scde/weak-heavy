using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnToPlayer : MonoBehaviour {


	public GameObject[] players;

	void FixedUpdate() {
		GameObject nextPlayer = findNearestPlayer ();
		print ("nextPlayer: " + nextPlayer.name);
		lookAtTarget (nextPlayer.transform);
		}

	GameObject findNearestPlayer ()
	{
		GameObject nextPlayer;
		Vector3 dirPlayerOne = players [0].transform.position - transform.position;
		Vector3 dirPlayerTwo = players [1].transform.position - transform.position;
		Vector3 dirPlayerOneNorm = dirPlayerOne.normalized;
		Vector3 dirPlayerTwoNorm = dirPlayerTwo.normalized;

		if (dirPlayerOneNorm.x < dirPlayerTwoNorm.x) {
			nextPlayer = players [0];
		} else {
			nextPlayer = players [1];
		}

		return nextPlayer;
	}

	void lookAtTarget(Transform target){
		Vector3 dir = target.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle -180, Vector3.forward);
	}
}
