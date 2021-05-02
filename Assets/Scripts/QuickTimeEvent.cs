using UnityEngine;
using System;
using TMPro;

public class QuickTimeEvent: MonoBehaviour {
    public TextMeshProUGUI display;

    //public string actionSequence;

    public System.Random random;

    public const double QTE_TIME = 10;
    public double timeLeft;
    int triggerCount;

    void Start() {
        /*
        string letters = "abcdefghijklmnopqrstuvwxyz";
        actionSequence = "";
        for (int i = 0; i < 3; i++) {
            actionSequence += letters[random.Next(letters.Length)];
        }
        */
        triggerCount = 3;
        RandomizePosition();
        timeLeft = QTE_TIME;
    }

    void Update() {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0) {
            PlayerStatistics.instance.Tax();
            Destroy(this.gameObject);
        }
    }

    public void RandomizePosition() {
        int xPos = random.Next(350+50) - 325;
        int yPos = random.Next(175+150) - 150;
        GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
    }

    public void QTETrigger() {
        triggerCount --;
        if (triggerCount <= 0) {
            Destroy(this.gameObject);
        } else {
            RandomizePosition();
        }
    }
}