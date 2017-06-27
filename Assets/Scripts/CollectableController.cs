using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour {

    public GameController gameController;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player")
        {
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
