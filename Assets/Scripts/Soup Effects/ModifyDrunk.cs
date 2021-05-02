using UnityEngine;

public class ModifyDrunk: MonoBehaviour {

    public int drunkMod;

    void Start() {
        PlayerStatistics.instance.drunkness += drunkMod;
        Destroy(gameObject);
    }

}