using UnityEngine;
using System;

public class MultiplyMoneyEffect: MonoBehaviour {

    public double multiplier;

    void Start() {
        PlayerStatistics.instance.money *= multiplier;
        Destroy(gameObject);
    }
}