using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequentAttack : MonoBehaviour {

	public float attackCooldown;

	private float nextAttackTime = 0.0f;

	private AttackController attackController;

	// Use this for initialization
	void Start () {
		attackController = GetComponentInChildren<AttackController> ();
	}
	
	// Update is called once per frame
	void Update () {
		nextAttackTime +=  Time.deltaTime;

		if (nextAttackTime >= attackCooldown) {
			nextAttackTime = nextAttackTime - attackCooldown;

			attackController.Attack ();
			Debug.Log ("Scissor Attacks");
		}

	}
}
