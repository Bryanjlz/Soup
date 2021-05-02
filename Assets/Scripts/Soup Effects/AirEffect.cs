using UnityEngine;
using System.Collections;

public class AirEffect: MonoBehaviour {

    void Start() {
        PlayerStatistics.instance.pGenTime *= 0.5f;
        StartCoroutine("Timer");
    }

    IEnumerator Timer() {
        yield return new WaitForSeconds(30);
        PlayerStatistics.instance.pGenTime *= 2f;  
        Destroy(gameObject);
    }
}