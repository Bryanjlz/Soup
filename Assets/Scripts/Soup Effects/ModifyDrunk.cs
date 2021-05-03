using UnityEngine;

public class ModifyDrunk: MonoBehaviour {

    public int drunkMod;
    public bool destroy = false;

    void Start() {
        PlayerStatistics.instance.drunkness += drunkMod;
        if (destroy) {
            Destroy(gameObject);
        }
    }

}