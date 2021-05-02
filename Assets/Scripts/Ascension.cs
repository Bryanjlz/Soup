using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Ascension {

    public int ascensions = 0;
    public int soupsSacrificed = 0;
    public double ascensionBonus = 1.00;

    public void Ascend(ref Dictionary<Soup, int> soups) {
        ascensions++; 
        foreach (Soup soup in soups.Keys) {
            soupsSacrificed += soups[soup];
            ascensionBonus += 0.001 * (soup.rarity * soup.rarity);
        }

        soups = new Dictionary<Soup, int>();
    }

    public double GetGachaModifier() {
        return Math.Pow(1.1, ascensions);
    }

    public double GetAscensionBonus() {
        return ascensionBonus;
    }
}