using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuController : MonoBehaviour
{

    private bool isShown;
    public bool IsShown
    {
        get
        {
            return isShown;
        }
        set
        {
            ShowMenu(value);
        }
    }

    public bool pausesGame;
    public GameObject firstSelected;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void ShowMenu(bool show)
    {
        isShown = show;
        anim.SetBool("IsOpen", isShown);
        StartCoroutine(SelectFirstSelected());
    }

    // HACK TODO maybe improve this (very hacky) (check if interactable state, etc. plays a role here)
    // Maybe it is even possible with just one call to SetSelectedGameObject
    private IEnumerator SelectFirstSelected()
    {
        float clipLength = Utilities.GetClipLength(anim, gameObject.name + "_Open");
        GUIController.Instance.EventSystem.SetSelectedGameObject(null);
        // the added delay seems to be needed
        yield return new WaitForSecondsRealtime(clipLength + 0.1f);
        GUIController.Instance.EventSystem.SetSelectedGameObject(firstSelected);
    }
}
