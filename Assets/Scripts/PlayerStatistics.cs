using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
        {
            instance = this;
        }
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
}
