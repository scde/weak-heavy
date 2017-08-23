using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTrigger : MonoBehaviour {

    public string popUpText;

    private void Start()
    {
        Debug.LogWarning("Warning: No text set!");
        popUpText = "Warning: No text set!";
    }

    private void OnTriggerEnter2D()
    {
        GUIController.Instance.ShowPaperRole(popUpText);
    }

    private void OnTriggerExit2D()
    {
        GUIController.Instance.HidePaperRole();
    }
}
