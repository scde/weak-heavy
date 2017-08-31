using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenPopUp : MonoBehaviour
{
    public string fullScreenPopUpText;

    private bool initialTrigger;

    private void Start()
    {
        if (fullScreenPopUpText == null)
        {
            Debug.LogWarning(gameObject + ": No pop up text set! Please set popUpText text in script options.");
        }
        initialTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (initialTrigger)
            {
                initialTrigger = false;
                TriggerPopUp();
            }
            if (col.gameObject == WeakController.Instance.gameObject)
            {
                EventManager.StartListening("Action_" + WeakController.Instance.PlayerId, TriggerPopUp);
            }
            else
            {
                EventManager.StartListening("Action_" + HeavyController.Instance.PlayerId, TriggerPopUp);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (col.gameObject == WeakController.Instance.gameObject)
            {
                EventManager.StopListening("Action_" + WeakController.Instance.PlayerId, TriggerPopUp);
            }
            else
            {
                EventManager.StopListening("Action_" + HeavyController.Instance.PlayerId, TriggerPopUp);
            }
        }
    }

    private void TriggerPopUp()
    {
        if (!GameManager.Instance.IsPaused)
        {
            GUIController.Instance.ShowFullScreenPopUp(fullScreenPopUpText);
        }
    }
}
