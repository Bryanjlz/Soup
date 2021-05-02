using UnityEngine;
using System.Collections;

public class DrunkHalfLife: MonoBehaviour {

    public float halfLife = 10;
    public int initialDrunk = 200;
    public int drunkPerHL = -50;
    public float taxTimerOverride = 1;

    //private double restoreTimer;


    Coroutine r;
    Coroutine rr;

    void Start() {
        PlayerStatistics.instance.drunkness += initialDrunk;
        //restoreTimer = Taxation.instance.TAX_TIMER;
        //Taxation.instance.TAX_TIMER = taxTimerOverride;
        //Taxation.instance.taxTimer = 0;
        r = StartCoroutine("HalfLife");
        rr = StartCoroutine("TaxesTaxes");
    }

    void OnDestroy() {
        StopCoroutine(r);
        StopCoroutine(rr);
    }

    IEnumerator TaxesTaxes() {
        for (int i = 0; i < 60; i++) {
            yield return new WaitForSeconds(taxTimerOverride);
            Taxation.instance.Tax();
        }
    }

    IEnumerator HalfLife() {
        for (int i = 0; i < 6; i++) {
            yield return new WaitForSeconds(halfLife);
            PlayerStatistics.instance.drunkness += drunkPerHL;
        }
        Destroy(gameObject);
    }
    
}