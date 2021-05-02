using UnityEngine;
using System;

public class Taxation: MonoBehaviour {

    public static Taxation instance;

    [Header("Taxation QTE Object")]
    public GameObject qtePopup;                 // Instantiate to trigger QTE
    public Transform canvas;                       // Canvas to act as parent to QTE

    [Header("Taxation Info")]
    public double taxTimer;                     // Tracks time until next taxation

    public double TAX_TIMER = 20;

    private System.Random random;              

    void Start() {
        if (!instance) {
            instance = this;
            taxTimer = TAX_TIMER;
            random = new System.Random();
        } else {
            Destroy(gameObject);
        }
    }

    public void Update() {
        taxTimer -= Time.deltaTime;

        if (taxTimer <= 0) {
            Tax();
            taxTimer = TAX_TIMER;
        }
    }

    public void Tax() {
        GameObject go = Instantiate(qtePopup, canvas);
        go.GetComponent<QuickTimeEvent>().random = random;
    }
}