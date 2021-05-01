using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerStatistics : MonoBehaviour
{
    // Singleton
    public static PlayerStatistics instance;

    // constants 
    public const int BASE_MONEY = 0;
    public const int BASE_CLICK_POWER = 0;
    public const int BASE_PASSIVE_POWER = 0;
    public const int BASE_TAX_PERCENTAGE = 0;
    public const float BASE_PASSIVE_GEN = 2.0f;


    // tracked values
    public int money = BASE_MONEY;
    public int clickPower = BASE_CLICK_POWER;
    public int passivePower = BASE_PASSIVE_POWER;
    public float taxPercentage = BASE_TAX_PERCENTAGE;

    // times
    public float pGenTime = BASE_PASSIVE_GEN;
    public float pGenTimer = 0f;

    // List of Soups
    public Dictionary<Soup, int> soups;
    //For editing only
    public List<Soup> collectedSoupList;

    // Debug List of All Soups
    public Soup[] allSoups;

    //RNG
    System.Random rng;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance) {
            instance = this;
            soups = new Dictionary<Soup, int>();
            rng = new System.Random();
            LoadAllSoup();
        } else {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        pGenTimer += Time.deltaTime;

        if (pGenTimer >= pGenTime) {
            money += passivePower;
            pGenTimer = 0f;
        }
    }

    public void Click() {
        money += clickPower;
    }

    public void AddSoup(Soup soup) {
        AddSoup(soup, 1);
    }

    public void AddSoup(Soup soup, int amount) {
        if (soups.ContainsKey(soup)) {
            soups[soup] += amount;
        } else {
            soups[soup] = amount;
        }
        for (int i = 0; i < amount; i++) {
            collectedSoupList.Add(soup);
        }
        RecalculateSoup();
    }

    public void RecalculateSoup() {
        double cp = BASE_CLICK_POWER;
        double pp = BASE_PASSIVE_POWER;
        double tp = BASE_TAX_PERCENTAGE;

        foreach (Soup soup in soups.Keys) {
            cp += soup.clickAdditive * soups[soup];
            pp += soup.passiveAdditive * soups[soup];
            tp += soup.taxAmountAdditive * soups[soup];
        }

        foreach (Soup soup in soups.Keys) {
            print(Math.Pow(1 + soup.clickMultiplicative, soups[soup]));
            print(cp);
            cp *= Math.Pow(1 + soup.clickMultiplicative, soups[soup]);
            pp *= Math.Pow(1 + soup.passiveMultiplicative, soups[soup]);
            tp *= Math.Pow(1 + soup.taxAmountMultiplicative, soups[soup]);
        }

        clickPower = (int) cp;
        passivePower = (int) pp;
        taxPercentage = (int) tp;
    }

    public void GainRandomSoup() {
        int r = rng.Next(allSoups.Length);
        AddSoup(allSoups[r], 1);
    }

    // Load all soup scriptable objs
    private void LoadAllSoup() {
        allSoups = Resources.LoadAll<Soup>("Soups");

        AddSoup(allSoups[13]);

        //Debug - seems to work for now
        /*
        foreach (Soup soup in allSoups) {
            clickPower += soup.clickAdditive;
            passivePower += soup.passiveAdditive;
        }
        */
    }
}


