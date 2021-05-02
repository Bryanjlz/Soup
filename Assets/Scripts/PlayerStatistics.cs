using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using System.Collections;

public class PlayerStatistics : MonoBehaviour
{
    // Singleton
    public static PlayerStatistics instance;

    [Header("Connected Text")]
    // Connected Displays
    public TextMeshProUGUI CPText;
    public TextMeshProUGUI PPText;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI CMText;
    public TextMeshProUGUI PMText;

    // constants 
    [Header("Constants")]
    public const int BASE_MONEY = 0;
    public const int BASE_CLICK_POWER = 0;
    public const int BASE_PASSIVE_POWER = 0;
    public const int BASE_TAX_PERCENTAGE = 0;
    public const float BASE_PASSIVE_GEN = 1.0f;

    // tracked values
    [Header("Tracked Values")]
    public double money = BASE_MONEY;
    public double clickPower = BASE_CLICK_POWER;
    public double clickMultiplier = 1f;
    public double passivePower = BASE_PASSIVE_POWER;
    public double passiveMultiplier = 1f;
    public float taxPercentage = BASE_TAX_PERCENTAGE;

    // times
    [Header("Times")]
    public float pGenTime = BASE_PASSIVE_GEN;
    public float pGenTimer = 0f;

    [Header("Lists (open if you dare)")]
    // List of Soups
    public Dictionary<Soup, int> soups;
    int totalSoup = 0;

    // Debug List of All Soups
    public Soup[] allSoups;

    //RNG
    public System.Random rng;

    // Scroll bar parent
    public GameObject content;
    public GameObject soupMenuPrefab;

    // Star Placement Constants
    public GameObject starPrefab;
    private const float STAR_X_START = -40f;
    private const float STAR_X_SPACE = 20f;
    private const float STAR_Y = -27;

    // Drunk stuff
    public int drunkness = 0;
    int xDest = 0;
    int yDest = 0;
    float speed = 3f;

    // Drunk Mouse stuff I don't understand
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out MousePosition lpMousePosition);

    public int xdmp;
    public int ydmp;
    [StructLayout(LayoutKind.Sequential)]
    public struct MousePosition {
        public int x;
        public int y;
    }

    // Auto Confirm
    public bool autoConfirm = false;

    public Ascension ascension;

    public List<GameObject> destroyOnAscend;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance) {
            instance = this;
            soups = new Dictionary<Soup, int>();
            rng = new System.Random();
            ascension = new Ascension();
            destroyOnAscend = new List<GameObject>();
            LoadAllSoup();
        } else {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void FixedUpdate() {
        speed = drunkness / 5f;
        if (speed > 10f) {
            speed = 10f;
        }
        MousePosition mp;
        GetCursorPos(out mp);
        if ((xDest <= 5 || yDest <= 5) && drunkness != 0) {
            xDest = rng.Next(drunkness);
            yDest = rng.Next(drunkness);

            xdmp = (int)(xDest / speed);
            ydmp = (int)(yDest / speed);

            if (xdmp == 0 && ydmp == 0 && drunkness != 0) {
                xdmp = 1;
                ydmp = 1;
            }

            if (rng.Next(2) == 0) {
                xdmp *= -1;
            }
            if (rng.Next(2) == 0) {
                ydmp *= -1;
            }
        }
        xDest -= Math.Abs(xdmp);
        yDest -= Math.Abs(ydmp);
        SetCursorPos(mp.x + xdmp, mp.y + ydmp);
    }

    // Update is called once per frame
    void Update()
    {
        pGenTimer += Time.deltaTime;

        if (pGenTimer >= pGenTime) {
            GainMoney(passivePower);
            pGenTimer = 0f;
        }

        GoldText.text = String.Format("Gold: {0}", BigNumberFormat(money, 1000000000));
    }

    public void Click() {
        GainMoney(clickPower);
        Vector2 clickStart = Input.mousePosition;
        clickStart.y += 1;
        GameObject moneyText = ObjectPool.SharedInstance.GetPooledObject();
        if (moneyText != null)
        {
            moneyText.transform.position = clickStart;
            moneyText.transform.rotation = this.transform.rotation;
            moneyText.GetComponent<TextMeshProUGUI>().text = "+" + Math.Round(clickPower) + "$";
            moneyText.SetActive(true);
            StartCoroutine(LateCall(moneyText));
        }
    }

    IEnumerator LateCall(GameObject moneyText)
    {

        yield return new WaitForSeconds(1f);

        moneyText.SetActive(false);
        //Do Function here...
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

        if (soup.associatedPrefab != null) {
            Instantiate(soup.associatedPrefab);
        }

        if (soup.soupName.Contains("Beer")) {
            drunkness += 1;
        }

        if (soup.soupName.Contains("Scotch")) {
            drunkness += 5;
        }

        if (soup.soupName.Contains("Vodka")) {
            drunkness += 10;
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

            // Set Soup Sprite
            newTab.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = soup.sprite;
            print(soup.description);

            // Set Soup Description
            if (!soup.description.Equals("")) {
                newTab.transform.GetChild(0).gameObject.GetComponent<CanMenuScript>().description = soup.description;
            } else {
                newTab.transform.GetChild(0).gameObject.GetComponent<CanMenuScript>().description = AutoGenerateDescription(soup);
            }

            // Truncate Soup name if name ends in soup
            string soupName = soup.soupName;
            if (soupName.Substring(soupName.LastIndexOf(" ") + 1, 4).ToLowerInvariant().Equals("soup")) {
                soupName = soupName.Substring(0, soupName.LastIndexOf(" "));
            }
            newTab.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "Type: " + soupName;

            // Set Amount
            newTab.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Number: " + amount;

            // Set Star Rarity
            CreateStars(soup, newTab.transform.GetChild(6));

            // Set parent, game object name, and scale
            newTab.name = soup.soupName;
            newTab.transform.SetParent(content.transform);
            newTab.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        RecalculateSoupPercents();
    }

    public string AutoGenerateDescription (Soup soup) {
        string d = "";
        if (soup.clickAdditive > 0) {
            d += "Makes your clicks give you " + soup.clickAdditive + " more gold. \n";
        }
        if (soup.clickMultiplicative > 0) {
            d += "Makes your clicks give you " + (100 * soup.clickMultiplicative) + "% more gold. \n";
        }
        if (soup.passiveAdditive > 0) {
            d += "Passively generates " + soup.passiveAdditive + " gold. \n";
        }
        if (soup.passiveMultiplicative > 0) {
            d += "Passively generates " + (100 * soup.passiveMultiplicative) + "% more gold. \n";
        }
        if (soup.taxAmountAdditive > 0) {
            d += "You are now taxed " + soup.taxAmountAdditive + " more gold. \n";
        }
        if (soup.taxAmountMultiplicative > 0) {
            d += "You are now taxed " + (100 * soup.taxAmountMultiplicative) + "% more gold. \n";
        }

        return d;
    }

    public void CreateStars (Soup soup, Transform starParent) {
        for (int i = 0; i < soup.rarity; i++) {
            GameObject newStar = Instantiate(starPrefab);
            newStar.name = "star";
            newStar.transform.SetParent(starParent);
            newStar.transform.localScale = new Vector3(1f, 1f, 1f);

            // Rect Transform
            RectTransform rt = newStar.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(16f, 16f);
            rt.localPosition = new Vector2(STAR_X_START + STAR_X_SPACE * i, STAR_Y);
        }
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

        clickMultiplier = 1f;
        passiveMultiplier = 1f;

        foreach (Soup soup in soups.Keys) {
            //print(Math.Pow(1 + soup.clickMultiplicative, soups[soup]));
            //print(cp);

            clickMultiplier *= Math.Pow(1 + soup.clickMultiplicative, soups[soup]);
            passiveMultiplier *= Math.Pow(1 + soup.passiveMultiplicative, soups[soup]);
            tp *= Math.Pow(1 + soup.taxAmountMultiplicative, soups[soup]);
        }
        cp *= clickMultiplier;
        pp *= passiveMultiplier;

        if (cp < 1) {
            cp = 1;
        }

        if (pp < 0)
        {
            pp = 0;
        }

        clickPower = cp * ascension.GetAscensionBonus();
        passivePower = pp * ascension.GetAscensionBonus();
        taxPercentage = (float) tp;
    }

    public void GainRandomSoup() {
        int r = rng.Next(allSoups.Length);
        AddSoup(allSoups[r], 1);
    }

    public void GainRandomSoup(int rarityLock) {
        int r = rng.Next(allSoups.Length);
        while (allSoups[r].rarity >= rarityLock) {
            r = rng.Next(allSoups.Length);
        }
        AddSoup(allSoups[r], 1);
    }

    // Load all soup scriptable objs
    private void LoadAllSoup() {
        allSoups = Resources.LoadAll<Soup>("Soups");

        AddSoup(allSoups[23]);

        //Debug - seems to work for now
        /*
        foreach (Soup soup in allSoups) {
            clickPower += soup.clickAdditive;
            passivePower += soup.passiveAdditive;
        }
        */
    }

    public void SetStatDisplay() {
        CPText.text = String.Format("CP: {0} Gold/Click", BigNumberFormat(clickPower, 1000000000.0));
        PPText.text = String.Format("PP: {0} Gold/s", BigNumberFormat(passivePower, 1000000000.0));
        CMText.text = String.Format("CM: {0}x", BigNumberFormat((100 * clickMultiplier)/100f, 10000));
        PMText.text = String.Format("PM: {0}x", BigNumberFormat((100 * passiveMultiplier)/100f, 100000));
    }

    private string BigNumberFormat(double bigNumber, double limit) {
        if (bigNumber < limit) {
            return bigNumber.ToString("F0");
        }
        return bigNumber.ToString("e2");
    }

    public void Tax() {
        double taxes = taxPercentage * money;
        LoseMoney(taxes);
        SetStatDisplay();
    }

    public void GainMoney (double gain) {
        money += gain;
        SetStatDisplay();
    }

    public void LoseMoney (double loss) {
        money -= loss;
        if (money < 0) {
            money = 0;
        }
        SetStatDisplay();
    }

    public void Ascend() {
        ascension.Ascend(ref soups);
        money = 0;
        foreach (Transform child in content.transform) {
            Destroy(child.gameObject);
        }

        foreach (GameObject go in destroyOnAscend) {
            Destroy(go);
        }
        destroyOnAscend = new List<GameObject>();

        AddSoup(allSoups[13]);
        
        RecalculateSoup();
        RecalculateSoupPercents();
        SetStatDisplay();
    }
}


