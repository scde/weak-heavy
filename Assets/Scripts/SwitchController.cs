using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes GameObject Switch:
// - Handles triggering
// - Handles effects (e.g. en-/disable, event firing (only?), etc.)
public class SwitchController : MonoBehaviour
{

    public GameObject[] affectedObjects;
    public bool isTriggerSwitch = false;

    private bool switchIsOn = false;
    private bool switchLocked = false;
	private bool switchable = true;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !switchLocked && isTriggerSwitch)
        {
            Switch();
        }
    }


    // RigidBody2Ds "Sleeping Mode"-option has to be set to "Never Sleep" otherwise the rb2d will go to sleep and stay will stop fireing
    private void OnTriggerStay2D(Collider2D col)
    {
		Debug.Log ("isInTrigger");
		if (col.gameObject.tag == "Player" && !switchLocked && !isTriggerSwitch && switchable)
        {
            // TODO Events & Listeners would be likely be better!
            if (col.GetComponent<WeakController>() && Input.GetButtonDown("Action_0"))
            {
				Debug.Log("switch activated");
                Switch();
            }
            else if (col.GetComponent<HeavyController>() && Input.GetButtonDown("Action_1"))
            {
				Debug.Log("switch activated");
                Switch();
            }
        }
    }

    private void Switch()
    {
        StartCoroutine(ShortInactivity());
        switchIsOn = !switchIsOn;

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, - transform.eulerAngles.z);
		if (transform.rotation.y == 0) {
			transform.rotation = Quaternion.Euler (0, 180, 0);
		} else if (transform.rotation.y == 180) {
			transform.rotation = Quaternion.Euler (0, 0, 0);
		}
		// disables switch if neccessary change to true
		switchable = false;

        //if (switchIsOn)
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
        //else
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 15);

        // disable/enable affectedObjects or do other stuff
        foreach (GameObject gObj in affectedObjects)
        {
            if (gObj.activeSelf)
            {
                gObj.SetActive(false);
            }
            else
            {
                gObj.SetActive(true);
            }
        }
    }

    private IEnumerator ShortInactivity()
    {
        switchLocked = true;
        // you can also realize timer switches here (you would likely need to change other stuff though)
        yield return new WaitForSeconds(1);
        switchLocked = false;
    }
}
