using UnityEngine;
using System;

public class Taxation: MonoBehaviour {

    [Header("Taxation QTE Object")]
    public GameObject qtePopup;                 // Instantiate to trigger QTE
    public Canvas canvas;                       // Canvas to act as parent to QTE

    [Header("Taxation Info")]
    public double taxTimer;                     // Tracks time until next taxation

    public const double TAX_TIMER = 20;

    private System.Random random;              

    void Start() {
        taxTimer = TAX_TIMER;
        random = new System.Random();
    }

    public void Update() {
        taxTimer -= Time.deltaTime;

        if (taxTimer <= 0) {
            GameObject go = Instantiate(qtePopup, canvas.transform);
            go.GetComponent<QuickTimeEvent>().random = random;
            taxTimer = TAX_TIMER;
        }
    }
}