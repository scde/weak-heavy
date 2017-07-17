using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes GameObject attackable/mortal:
// - Keeps track of health
// - Keeps track of invincibility
// - Handles death (fires death event?)
public class HealthController : MonoBehaviour {

    public float maxHealth = 100.0f;

    float currentHealth;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage (float damage) {
        currentHealth -= damage;
        // Display Stuff (on HUD, Avatar[red flash], etc.)
        if (currentHealth <= 0.0f) {
            Destroy(gameObject);
        }
    }
}
