using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanMenuScript : MonoBehaviour
{
    public string description;
    GameObject tooltip;
    public Button button;

    private void Start() {
        tooltip = GameObject.Find("Tooltip");
    }

    public void ShowIfDisabled() {
        if (button.interactable == false) {
            ShowTooltip();
        }
    }

    public void ShowTooltip() {
        tooltip.GetComponent<Tooltip>().ShowTooltip(description);
    }

    public void HideTooltip() {
        tooltip.GetComponent<Tooltip>().HideTooltip();
    }
}
