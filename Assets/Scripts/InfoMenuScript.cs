using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenuScript : MonoBehaviour
{
    public string text;
    public Sprite image;
    public bool showOnFirstEnter;
    public bool showOnEnter;
    public bool showOnActionPress;

    private MenuController infoMenu;
    private Text infoText;
    private Text infoTextOnly;
    private Image infoImage;
    private bool initialTrigger;

    private void Start()
    {
        if (text == null)
        {
            Debug.LogWarning(gameObject + ": No text set! Please set it in info menu script options.");
        }

        foreach (MenuController menu in GUIController.Instance.Menus)
        {
            switch (menu.name)
            {
                case "InfoMenu":
                    infoMenu = menu;
                    break;
            }
        }
        foreach (Text t in infoMenu.GetComponentsInChildren<Text>())
        {
            switch (t.name)
            {
                case "InfoText":
                    infoText = t;
                    break;
                case "InfoTextOnly":
                    infoTextOnly = t;
                    break;
            }
        }
        foreach (Image i in infoMenu.GetComponentsInChildren<Image>())
        {
            switch (i.name)
            {
                case "InfoImage":
                    infoImage = i;
                    break;
            }
        }

        initialTrigger = showOnFirstEnter;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (initialTrigger || showOnEnter)
            {
                initialTrigger = false;
                TriggerPopUp();
            }

            if (showOnActionPress)
            {
                if (col.gameObject == WeakController.Instance.gameObject)
                {
                    EventManager.Instance.StartListening("Action_" + WeakController.Instance.PlayerId, TriggerPopUp);
                }
                else
                {
                    EventManager.Instance.StartListening("Action_" + HeavyController.Instance.PlayerId, TriggerPopUp);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" && showOnActionPress)
        {
            if (col.gameObject == WeakController.Instance.gameObject)
            {
                EventManager.Instance.StopListening("Action_" + WeakController.Instance.PlayerId, TriggerPopUp);
            }
            else
            {
                EventManager.Instance.StopListening("Action_" + HeavyController.Instance.PlayerId, TriggerPopUp);
            }
        }
    }

    private void ResetUIElements()
    {
        infoImage.enabled = true;
        infoText.enabled = true;
        infoTextOnly.enabled = true;
    }

    private void TriggerPopUp()
    {
        ResetUIElements();
        // Allow escape characters like line breaks ("\n") in passed string
        // source: https://forum.unity3d.com/threads/inputing-a-line-break-in-a-text-field-for-ui.319223/#post-3077848
        string unescapedText = System.Text.RegularExpressions.Regex.Unescape(text);
        if (image != null)
        {
            infoImage.sprite = image;
            infoText.text = unescapedText;
            infoTextOnly.enabled = false;
        }
        else
        {
            infoTextOnly.text = unescapedText;
            infoImage.enabled = false;
            infoText.enabled = false;
        }
        GUIController.Instance.ShowMenu(infoMenu);
    }
}
