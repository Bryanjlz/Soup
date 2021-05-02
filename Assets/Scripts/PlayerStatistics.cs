using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlayerStatistics : MonoBehaviour
{
    // Singleton
    public static PlayerStatistics instance;

    [Header("Connected Text")]
    // Connected Displays
    public TextMeshProUGUI CPText;
    public TextMeshProUGUI PPText;
    public TextMeshProUGUI GoldText;

    // constants 
    [Header("Constants")]
    public const int BASE_MONEY = 0;
    public const int BASE_CLICK_POWER = 0;
    public const int BASE_PASSIVE_POWER = 0;
    public const int BASE_TAX_PERCENTAGE = 0;
    public const float BASE_PASSIVE_GEN = 2.0f;

    // tracked values
    [Header("Tracked Values")]
    public int money = BASE_MONEY;
    public int clickPower = BASE_CLICK_POWER;
    public int passivePower = BASE_PASSIVE_POWER;
    public float taxPercentage = BASE_TAX_PERCENTAGE;

    // times
    [Header("Times")]
    public float pGenTime = BASE_PASSIVE_GEN;
    public float pGenTimer = 0f;

    [Header("Lists (open if you dare)")]
    // List of Soups
    public Dictionary<Soup, int> soups;
    int totalSoup = 0;

    //For editing only
    public List<Soup> collectedSoupList;

    // Debug List of All Soups
    public Soup[] allSoups;

    //RNG
    public System.Random rng;

    // Scroll bar parent
    public GameObject content;
    public GameObject soupMenuPrefab;

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

        GoldText.text = String.Format("Gold: {0}", money);
    }

    public void Click() {
        money += clickPower;
    }

    public void AddSoup(Soup soup) {
        AddSoup(soup, 1);
    }

    public void AddSoup(Soup soup, int amount) {
        totalSoup += amount;
        // Update scroll menu
        AddSoupToMenu(soup, amount);

        if (soups.ContainsKey(soup)) {
            soups[soup] += amount;
        } else {
            soups[soup] = amount;
        }

        for (int i = 0; i < amount; i++) {
            collectedSoupList.Add(soup);
        }
        RecalculateSoup();
        SetStatDisplay();
    }

    private void AddSoupToMenu (Soup soup, int amount) {
        // If soup exists, just update the tab
        if (soups.ContainsKey(soup)) {
            // Find gameobject (or rather the transform of object)
            Transform tab = content.transform.Find(soup.soupName);

            // Set Soup values
            tab.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Number: " + (soups[soup] + amount);

        } else {
            // Instantiate prefab
            GameObject newTab = Instantiate(soupMenuPrefab);

            // Set Soup values
            newTab.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = soup.sprite;
            newTab.transform.GetChild(0).gameObject.GetComponent<CanMenuScript>().description = soup.description;

            // Truncate Soup name if name ends in soup
            string soupName = soup.soupName;
            if (soupName.Substring(soupName.LastIndexOf(" ") + 1, 4).ToLowerInvariant().Equals("soup")) {
                soupName = soupName.Substring(0, soupName.LastIndexOf(" "));
            }
            newTab.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "Type: " + soupName;

            // Set Amount
            newTab.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Number: " + amount;

            // Set parent, game object name, and scale
            newTab.name = soup.soupName;
            newTab.transform.SetParent(content.transform);
            newTab.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        RecalculateSoupPercents();
    }

    public void RecalculateSoupPercents () {
        // Update all percents
        foreach (Transform tab1 in content.transform) {
            string number = tab1.GetChild(2).gameObject.GetComponent<TMP_Text>().text;
            int amount1 = int.Parse(number.Substring(number.IndexOf(" ") + 1));
            tab1.GetChild(3).gameObject.GetComponent<TMP_Text>().text = "Percent: " + Math.Round(100f * ((float)amount1 / totalSoup)) + "%";
        }
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
            //print(Math.Pow(1 + soup.clickMultiplicative, soups[soup]));
            //print(cp);
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

    public void SetStatDisplay() {
        CPText.text = String.Format("CP: {0}", clickPower);
        PPText.text = String.Format("PP: {0}", passivePower);
    }

    
    public void Tax() {
        int taxes = (int) (taxPercentage * money);
        int newVal = money - taxes;
        if (newVal <= 0) {
            newVal = 0;
        }
        money = newVal;
        SetStatDisplay();
    }
}


