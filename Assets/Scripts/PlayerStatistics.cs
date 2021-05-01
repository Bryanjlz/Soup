using UnityEngine;
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
    public Dictionary<string, int> soups;

    public Soup[] allSoups;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance) {
            instance = this;
            soups = new Dictionary<string, int>();
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

    public void AddSoup(string soup, int amount) {
        if (soups.ContainsKey(soup)) {
            soups[soup] += amount;
        } else {
            soups[soup] = amount;
        }
    }

    // Load all soup scriptable objs
    private void LoadAllSoup() {
        allSoups = Resources.LoadAll<Soup>("Soups");

        //Debug - seems to work for now
        /*
        foreach (Soup soup in allSoups) {
            clickPower += soup.clickAdditive;
            passivePower += soup.passiveAdditive;
        }
        */
    }
}


