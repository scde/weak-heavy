using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnToPlayer : MonoBehaviour {


	public GameObject[] players;


	private bool facingRight = true;	


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		GameObject nextPlayer = findNearestPlayer ();
		Vector3 dir = nextPlayer.transform.position - transform.position;
		print ("nextPlayer: " + nextPlayer.name);

		if (dir.x < 0 && facingRight) {
			Flip ();
		} else if (dir.x > 0 && !facingRight) {
			Flip ();
		} else {
			// do nothing
		}


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

	private void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
