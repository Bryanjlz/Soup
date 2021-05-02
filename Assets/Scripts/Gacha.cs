﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gacha : MonoBehaviour
{
    public GachaRate[] gachaRates;
    public GameObject gottenThing;
    public string gachaName;

    public Button low, med, high, roll;

    public TMP_Text cost;

    private GachaRate curGacha;

    private void Start() {
        gachaName = "Low";
        LoadGacha();
        
        // Lazy code
        ChangeGacha("Low");
        
        low.gameObject.GetComponent<CanMenuScript>().description = AutoDescription(curGacha);
        ChangeGacha("Medium");
        med.gameObject.GetComponent<CanMenuScript>().description = AutoDescription(curGacha);
        ChangeGacha("High");
        high.gameObject.GetComponent<CanMenuScript>().description = AutoDescription(curGacha);

        ChangeGacha("Low");
    }

    private void LoadGacha() {
        gachaRates = Resources.LoadAll<GachaRate>("Gacha Rates");
    }

    private void Update() {
        if (curGacha.cost > PlayerStatistics.instance.money) {
            roll.interactable = false;
        } else {
            roll.interactable = true;
        }
    }

    public void ChangeGacha(string newName) {
        // Set gacha mame
        gachaName = newName;

        // Enable all buttons
        low.interactable = true;
        med.interactable = true;
        high.interactable = true;

        Button curButton;

        // Find Button
        if (gachaName.Equals("Low")) {
            curButton = low;
        } else if (gachaName.Equals("Medium")) {
            curButton = med;
        } else {
            curButton = high;
        }

        // Set button inactive
        curButton.interactable = false;

        // Find the gacha
        foreach (GachaRate gacha in gachaRates) {
            if (gacha.name.Equals(gachaName)) {
                curGacha = gacha;
            }
        }


        // Set cost text
        cost.text = "One roll costs:\n" + curGacha.cost + " Gold";

        
    }

    public string AutoDescription (GachaRate gachaRate) {
        string res = "";
        for (int i = 0; i < gachaRate.rates.Length; i++) {
            int star = i;
            if (i == 5) {
                star = 6;
            }
            res += (star + 1) + " Star: " + gachaRate.rates[i] + "% \n ";
        }
        res = res.Substring(0, res.LastIndexOf("\n"));
        return res;
    }


    public void RollGacha () {

        PlayerStatistics.instance.LoseMoney(curGacha.cost);

        int rand = PlayerStatistics.instance.rng.Next(100);
        int rate = 0;
        for (int i = 0; i < curGacha.rates.Length; i++) {
            rate += curGacha.rates[i];
            if (rand < rate) {
                int rarity = i + 1;
                if (rarity == 6) {
                    rarity = 7;
                }
                Soup gotten = GetSoupWithRarity(rarity);
                PlayerStatistics.instance.AddSoup(gotten);
                gottenThing.gameObject.GetComponent<Image>().sprite = gotten.sprite;
                gottenThing.gameObject.GetComponent<CanMenuScript>().description = PlayerStatistics.instance.AutoGenerateDescription(gotten);
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

}
