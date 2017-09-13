using System;
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
    public bool triggerOnEmpty = false;
    public bool isCheckpointTrigger;
    public float enableTime = 0.0f;
    public float disableTime = 0.0f;
    public float inactivityTime = 1.0f;
    public LayerMask activationLayerMask;
    public ItemID[] unlockWeakItems;
    public ItemID[] unlockHeavyItems;
    public ItemID[] lockWeakItems;
    public ItemID[] lockHeavyItems;

    private Animator anim;
    private bool switchIsOn = false;
    private bool switchLocked = false;
    private int activatorCounter;
    private Transform respawnPointWeak;
    private Transform respawnPointHeavy;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if (activationLayerMask == 0)
        {
            Debug.LogWarning(gameObject + ": No Activation Layer Mask set. Defaulting to both player characters.");
            activationLayerMask = LayerMask.GetMask(new string[] { "Player_Weak", "Player_Heavy" });
        }

        if (isCheckpointTrigger)
        {
            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                switch (t.name)
                {
                    case "RespawnPointWeak":
                        respawnPointWeak = t;
                        break;
                    case "RespawnPointHeavy":
                        respawnPointHeavy = t;
                        break;
                }
            }
            if (respawnPointWeak == null)
            {
                Debug.LogError(gameObject + ": Respawn point for Weak is not created yet. Create an empty Child-GameObject called \"RespawnPointWeak\"");
            }
            if (respawnPointHeavy == null)
            {
                Debug.LogError(gameObject + ": Respawn point for Heavy is not created yet. Create an empty Child-GameObject called \"RespawnPointHeavy\"");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (Utilities.CheckLayerMask(activationLayerMask, col.gameObject))
        {
            if (activatorCounter == 0 && triggerOnEmpty)
            {
                Switch();
            }
            activatorCounter++;
        }
        if (isTriggerSwitch && Utilities.CheckLayerMask(activationLayerMask, col.gameObject))
        {
            Switch();
        }

        if (!isTriggerSwitch && Utilities.CheckLayerMask(activationLayerMask, col.gameObject))
        {
            if (col.gameObject == WeakController.Instance.gameObject)
            {
                EventManager.Instance.StartListening("Action_" + WeakController.Instance.PlayerId, Switch);
            }
            else if (col.gameObject == HeavyController.Instance.gameObject)
            {
                EventManager.Instance.StartListening("Action_" + HeavyController.Instance.PlayerId, Switch);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (Utilities.CheckLayerMask(activationLayerMask, col.gameObject))
        {
            activatorCounter--;
            if (activatorCounter == 0 && triggerOnEmpty)
            {
                Switch();
            }
        }
        if (!isTriggerSwitch && Utilities.CheckLayerMask(activationLayerMask, col.gameObject))
        {
            if (col.gameObject == WeakController.Instance.gameObject)
            {
                EventManager.Instance.StopListening("Action_" + WeakController.Instance.PlayerId, Switch);
            }
            else if (col.gameObject == HeavyController.Instance.gameObject)
            {
                EventManager.Instance.StopListening("Action_" + HeavyController.Instance.PlayerId, Switch);
            }
        }
    }

    private void Switch()
    {
        if (switchLocked)
        {
            return;
        }

        StartCoroutine(InactivityTimer());

        if (enableTime > 0.0f)
        {
            StartCoroutine(EnableTimer());
        }
        else
        {
            SwapAffected();

            if (disableTime > 0.0f)
            {
                StartCoroutine(DisableTimer());
            }
        }
    }

    // possible TODO: Trigger animations, sounds
    // or possibly create dynamic events that just trigger here and can be executed elsewhere
    private void SwapAffected()
    {
        switchIsOn = !switchIsOn;
        if (anim != null)
        {
            anim.SetBool("Activated", switchIsOn);
        }

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

        // unlock or lock items
        ChangeLockOnItems();

        // set new checkpoint
        if (isCheckpointTrigger)
        {
            GameManager.Instance.CheckpointWeak = respawnPointWeak;
            GameManager.Instance.CheckpointHeavy = respawnPointHeavy;
        }
    }

    private void ChangeLockOnItems()
    {
        foreach (ItemID id in unlockWeakItems)
        {
            WeakController.Instance.ItemController.UnlockItem(id);
        }
        foreach (ItemID id in unlockHeavyItems)
        {
            HeavyController.Instance.ItemController.UnlockItem(id);
        }
        foreach (ItemID id in lockWeakItems)
        {
            WeakController.Instance.ItemController.LockItem(id);
        }
        foreach (ItemID id in lockHeavyItems)
        {
            HeavyController.Instance.ItemController.LockItem(id);
        }
    }

    private IEnumerator EnableTimer()
    {
        yield return new WaitForSeconds(enableTime);
        SwapAffected();

        if (disableTime > 0.0f)
        {
            StartCoroutine(DisableTimer());
        }
    }

    private IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(disableTime);
        SwapAffected();
    }

    private IEnumerator InactivityTimer()
    {
        switchLocked = true;
        yield return new WaitForSeconds(inactivityTime);
        if (!isOneTimeSwitch)
        {
            switchLocked = false;
        }
    }
}
