using UnityEngine;

[CreateAssetMenu(fileName="Soup", menuName= "Soup")]

public class Soup: ScriptableObject
{
    public string soupName;
    public int rarity;
    public Sprite sprite;

    public int clickAdditive;
    public int passiveAdditive;
    public int taxAmountAdditive;

    public int clickMultiplicative;
    public int passiveMultiplicative;
    public int taxAmountMultiplicative;

    // OPTIONAL!!!
    public GameObject associatedPrefab;
}