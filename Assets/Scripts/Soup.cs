using UnityEngine;

[CreateAssetMenu(fileName="Soup", menuName= "Soup")]

public class Soup: ScriptableObject
{
    public string soupName;
    public string description;
    public int rarity;
    public Sprite sprite;

    public double clickAdditive;
    public double passiveAdditive;
    public double taxAmountAdditive;

    public double clickMultiplicative;
    public double passiveMultiplicative;
    public double taxAmountMultiplicative;

    // OPTIONAL!!!
    public GameObject associatedPrefab;
}