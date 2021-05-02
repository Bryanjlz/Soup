using UnityEngine;
using System.Collections;

public class ExplodeEffect: MonoBehaviour {

    float timer;
    Coroutine r;

    void Start() {
        PlayerStatistics.instance.destroyOnAscend.Add(gameObject);
        timer = PlayerStatistics.instance.rng.Next(120) + 120;
        r = StartCoroutine("Explode");
    }

    void OnDestroy() {
        StopCoroutine(r);
    }

    IEnumerator Explode() {
        yield return new WaitForSeconds(timer);
        PlayerStatistics.instance.money *= 0.5;
    }
}