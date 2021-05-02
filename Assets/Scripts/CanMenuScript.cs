using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMenuScript : MonoBehaviour
{
    public string description;
    GameObject tooltip;

    private void Start() {
        tooltip = GameObject.Find("Tooltip");
    }

    public void ShowTooltip() {
        tooltip.GetComponent<Tooltip>().ShowTooltip(description);
    }

    public void HideTooltip() {
        print("hide");
        tooltip.GetComponent<Tooltip>().HideTooltip();
    }
}
