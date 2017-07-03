using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : MonoBehaviour {

    public GameObject respawnWeak;
    public GameObject respawnHeavy;

    // setting this should maybe be done with the event system
    // tutorial: https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
    private bool gameOver = false;

    private void OnTriggerExit2D(Collider2D col) {
        // TODO deactivate/activate, respawn, gameover sollten in den zuständigen scripts gehandlet werden (wshl. PlayerController oder GameController)
		if (col.tag == "Player") {
            if (gameOver && col.gameObject.activeSelf) {
    			col.gameObject.SetActive(false); // use this when you want to temporary disable a gameobject
			}
            if (col.gameObject.name == "Player_Weak") {
                col.gameObject.transform.position = respawnWeak.transform.position;
            }
			if (col.gameObject.name == "Player_Heavy")
			{
                col.gameObject.transform.position = respawnHeavy.transform.position;
			}
		}
        else {
            Destroy(col.gameObject);
        }
    }
}
