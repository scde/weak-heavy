using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes GameObject Collectable:
// - Holds collectable info (key-value pair?)
// - Handles collection/destruction (fires event?)
public class CollectableController : MonoBehaviour {

    public GameController gameController;

    // Fixes double count on double collision
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player" && !isCollected)
        {
            isCollected = true;
			//if (gameObject.activeSelf) // use this when you want to temporary disable a gameobject
			//{
			//gameObject.SetActive(false);
			//}
			// TODO: add item to inventory
			gameController.AddCoin();
			Destroy(gameObject);
		}
    }
}
