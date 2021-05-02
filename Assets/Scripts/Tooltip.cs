using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public TMP_Text text;
    public RectTransform background;
    public Camera cam;

    private bool isShown = true;

    public float textPadding = 4f;
    public int charLimit = 15;

    private void Start() {
        HideTooltip();
    }

    private void Update() {
        // Update position of tooltip to be on mouse
        if (isShown) {
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, cam, out local);
            transform.localPosition = local;
        }
    }

    public void ShowTooltip(string ttstring) {
        isShown = true;

        // Set text
        text.text = parseString(ttstring);

        // Set size of background
        background.sizeDelta = new Vector2(text.preferredWidth + textPadding * 2f, text.preferredHeight + textPadding * 2f);
    }

    public void HideTooltip() {
        isShown = false;
        background.sizeDelta = new Vector2(0, 0);
        text.text = "";
    }

    private string parseString (string ttstring) {
        // temp char limit that's subject to change if very long word
        int curCharLimit = charLimit;

        string result = "";

        // Parse each line
        while (ttstring.Length > 0) {
            // Vreates a line
            string line = "";
            bool doneLine = false;

            // Parse each word
            while (!doneLine) {
                // Get word
                string word = ttstring.Substring(0, ttstring.IndexOf(" ") + 1);

                // If no space found, there is only one word left
                if (word.Length == 0) {
                    word = ttstring;
                }

                // If word is too long, change char limit
                if (word.Length > curCharLimit) {
                    curCharLimit = word.Length;
                }

                // If found line ending or adding word goes over char limit, finish line
                if (word.Contains("\n") || word.Length + line.Length > curCharLimit) {
                    if (word.Contains("\n")) {
                        ttstring = ttstring.Substring(ttstring.IndexOf("\n") + 1);
                    }
                    line += "\n";
                    doneLine = true;
                } else {
                    // Add word to line
                    line += word;

                    // Check if it's the last word
                    if (ttstring.IndexOf(" ") < 0) {
                        ttstring = "";
                        doneLine = true;
                    }

                    // Update input string
                    ttstring = ttstring.Substring(ttstring.IndexOf(" ") + 1);
                }
            }
            // Add line to result
            result += line;
        }
        return result;
    }
}
