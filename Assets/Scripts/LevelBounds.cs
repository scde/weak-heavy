using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : MonoBehaviour {

    public GameObject respawnWeak;
    public GameObject respawnHeavy;

    private bool gameOver = false;

    private void OnTriggerExit2D(Collider2D col) {
        // TODO deactivate/activate, respawn, gameover sollten in den zuständigen scripts gehandlet werden (wshl. PlayerController oder GameController)
		if (col.tag == "Player") {
            if (col.gameObject.activeSelf) { // use this when you want to temporary disable a gameobject
    			col.gameObject.SetActive(false);
			}
            if (col.gameObject.name == "Player_Weak") {
                col.gameObject.transform.position = respawnWeak.transform.position;
            }
			if (col.gameObject.name == "Player_Heavy")
			{
                col.gameObject.transform.position = respawnHeavy.transform.position;
			}
			if (!gameOver) {
				col.gameObject.SetActive(true);
			}
		}
        else {
            Destroy(col.gameObject);
        }
    }
}
