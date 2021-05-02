using UnityEngine;

public class TaxEvasion: MonoBehaviour {

    public double taxTimeMod;
    public double taxIntervalMod;

    void Start() {
        Taxation.instance.TAX_TIMER += taxIntervalMod;
        Taxation.instance.taxTimer += taxTimeMod;
        Destroy(gameObject);
    }
}