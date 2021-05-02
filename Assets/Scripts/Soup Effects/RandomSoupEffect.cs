using UnityEngine;
using System.Collections;

public class RandomSoupEffect: MonoBehaviour {

    public float buyInterval;

    Coroutine r;

    void Start() {
        r = StartCoroutine("BuyBuyBuy");
        PlayerStatistics.instance.destroyOnAscend.Add(gameObject);
    }

    void OnDestroy() {
        StopCoroutine(r);
    }


    IEnumerator BuyBuyBuy() {
        while (true) {
            yield return new WaitForSeconds(buyInterval);
            PlayerStatistics.instance.GainRandomSoup(4);
        }
    }

}