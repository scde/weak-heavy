using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make use of this when creating a true RespawnController Script
public class LevelBounds : MonoBehaviour {

    public GameObject respawnWeak;
    public GameObject respawnHeavy;

    // setting this should maybe be done with the event system
    // tutorial: https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
    private bool gameOver = false;

    private void OnTriggerExit2D(Collider2D col) {
        // WARNING: en-/disabling colliders (with a Rigidbody2D) also evokes OnTriggerExit2D!!
        // TODO deactivate/activate, respawn, gameover sollten in den zuständigen scripts gehandlet werden (wshl. PlayerController oder GameController)
		if (col.tag == "Player") {
            if (gameOver && col.gameObject.activeSelf) {
    			col.gameObject.SetActive(false); // use this when you want to temporary disable a gameobject
			}
            if (col.gameObject.name == "PlayerWeak") {
                col.gameObject.transform.position = respawnWeak.transform.position;
            }
			if (col.gameObject.name == "Player_Heavy")
			{
                col.gameObject.transform.position = respawnHeavy.transform.position;
			}
		}
        else {
            print("I just left the Levelbounds " + col.gameObject.ToString());
            //Destroy(col.gameObject);
        }
    }
}
