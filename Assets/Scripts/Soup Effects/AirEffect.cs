using UnityEngine;
using System.Collections;

public class AirEffect: MonoBehaviour {

    Coroutine r;

    void Start() {
        PlayerStatistics.instance.pGenTime *= 0.5f;
        r = StartCoroutine("Timer");

        PlayerStatistics.instance.destroyOnAscend.Add(gameObject);
    }

    void OnDestroy() {
        // I think this prevents cursed timer modifications
        StopCoroutine(r);
    }

    IEnumerator Timer() {
        yield return new WaitForSeconds(30);
        PlayerStatistics.instance.pGenTime *= 2f;  
        Destroy(gameObject);
    }
}