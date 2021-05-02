using UnityEngine;

public class OrangeGuiceEffect: MonoBehaviour {

    void Start() {
        PlayerStatistics.instance.Ascend();

        Destroy(this.gameObject);
    }
}