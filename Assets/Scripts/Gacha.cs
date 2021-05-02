﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    public GachaRate[] gachaRates;

    private void Start() {
        LoadGacha();
    }

    public void RollGacha (string gachaName) {
        GachaRate curGacha = null;

        // Find the gacha
        foreach (GachaRate gacha in gachaRates) {
            if (gacha.name.Equals(gachaName)) {
                curGacha = gacha;
            }
        }

        int rand = PlayerStatistics.instance.rng.Next(100);
        int rate = 0;
        for (int i = 0; i < curGacha.rates.Length; i++) {
            rate += curGacha.rates[i];
            print(rate);
            if (rand < rate) {
                int rarity = i + 1;
                if (rarity == 6) {
                    rarity = 7;
                }
                print(rarity);
                Soup gotten = GetSoupWithRarity(rarity);
                PlayerStatistics.instance.AddSoup(gotten);
                break;
            }
        }

    }

    private Soup GetSoupWithRarity(int rarity) {
        Soup[] soups = PlayerStatistics.instance.allSoups;
        Soup result = null;
        while (result == null) {
            int rand = PlayerStatistics.instance.rng.Next(soups.Length);
            result = soups[rand];
            if (result.rarity != rarity) {
                result = null;
            }
        }
        return result;
    }

    private void LoadGacha () {
        gachaRates = Resources.LoadAll<GachaRate>("Gacha Rates");
    }

}
