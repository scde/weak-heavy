using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	public float timeBetweenBullets = 0.15f;
	public GameObject projectile;

	float nextBullet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake () {
		nextBullet = 0f;
	}
}
