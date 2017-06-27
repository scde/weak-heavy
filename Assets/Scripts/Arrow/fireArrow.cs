using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	public float timeBetweenBullets = 0.15f;
	public GameObject arrow;

	float nextArrow;

	void Awake () {
		nextArrow = 0f;
	}

	void Update () {
		HeavyController myPlayer = transform.root.GetComponent<HeavyController> ();

		if (Input.GetAxisRaw ("Fire1") > 0 && nextArrow < Time.time) {
			nextArrow = Time.time + timeBetweenBullets;
			Vector3 rot;
			if (myPlayer.GetFacing () == -1f) {
				rot = new Vector3 (0, -90, 0);
			} else {
				rot = new Vector3 (0, 90, 0);
			}

			Instantiate (arrow, transform.position, Quaternion.Euler (rot));
		}
	}
}
