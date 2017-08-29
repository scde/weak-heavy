using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpTrigger : MonoBehaviour
{
    private bool showPopUp;
    private Canvas popUp;
    private Animator paperRoleAnimator;
    private Text paperRoleText;
    private bool weakTrigger;
    private bool heavyTrigger;

    private void Start()
    {
        popUp = GetComponentInChildren<Canvas>();
        paperRoleAnimator = popUp.GetComponentInChildren<Animator>();
        paperRoleAnimator.gameObject.SetActive(false);
        paperRoleText = popUp.GetComponentInChildren<Text>();
        paperRoleText.enabled = false;

        if (paperRoleText.text == "")
        {
            Debug.LogWarning(gameObject + ": No text set! Please set Text in \"paperRoleText\" child.");
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (col.gameObject == WeakController.Instance.gameObject)
            {
                weakTrigger = true;
            }
            else
            {
                heavyTrigger = true;
            }
        }

        if (weakTrigger || heavyTrigger && !showPopUp)
        {
            showPopUp = true;
            StartCoroutine(SetPopUp(showPopUp));
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (col.gameObject == WeakController.Instance.gameObject)
            {
                weakTrigger = false;
            }
            else
            {
                heavyTrigger = false;
            }
        }

        if (!weakTrigger && !heavyTrigger && showPopUp)
        {
            showPopUp = false;
            StartCoroutine(SetPopUp(showPopUp));
        }
    }

    private IEnumerator SetPopUp(bool show)
    {
        float clipLength = 0.0f;
        RuntimeAnimatorController ac = paperRoleAnimator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == "Open_Close")
            {
                clipLength = ac.animationClips[i].length;
                break;
            }
        }

        if (show)
        {
            paperRoleAnimator.gameObject.SetActive(true);
        }
        else
        {
            paperRoleText.enabled = false;
        }
        // FIXME can still show text over an opening paperRole
        paperRoleAnimator.SetBool("Open", show);
        yield return new WaitForSeconds(clipLength);
        // global variable is needed to prevent false display state after yield
        if (show == showPopUp)
        {
            if (show)
            {
                paperRoleText.enabled = true;
            }
            else
            {
                paperRoleAnimator.gameObject.SetActive(false);
            }
        }
    }
}
