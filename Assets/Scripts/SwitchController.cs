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
    public bool isOneTimeSwitch = false;
    public float inactivityTime = 1.0f;

    private Animator anim;
    private bool switchIsOn = false;
    private bool switchLocked = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

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
        if (col.gameObject.tag == "Player" && !switchLocked && !isTriggerSwitch)
        {
            // TODO Events & Listeners would likely be better!
            if (col.GetComponent<WeakController>() && Input.GetButtonDown("Action_0"))
            {
                Switch();
            }
            else if (col.GetComponent<HeavyController>() && Input.GetButtonDown("Action_1"))
            {
                Switch();
            }
        }
    }

    private void Switch()
    {
        StartCoroutine(ShortInactivity());
        switchIsOn = !switchIsOn;
        anim.SetBool("Activated", switchIsOn);

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
        yield return new WaitForSeconds(inactivityTime);
        if (!isOneTimeSwitch) {
            switchLocked = false;
        }
    }
}
