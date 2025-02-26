using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class SayingRandomizer : MonoBehaviour {
    private TextMeshProUGUI sayingText;

    private string[] sayingList = new[] { "saying_01", "saying_02", "saying_03", "saying_04" };

    private LinkedList<string> pastSaying = new LinkedList<string>();
    private int maxPastSaying = 3;

    private void Awake() {
        sayingText = GetComponent<TextMeshProUGUI>();
        sayingText.text = string.Empty;
    }

    private void Start() {
        StartCoroutine(Randomizer());
    }

    private IEnumerator Randomizer() {
        while (true) {  // Run continuously
            int sayingIndex;
            bool validSaying;
            
            do {
                validSaying = true;
                sayingIndex = Random.Range(0, sayingList.Length);
                
                // Check if the saying was recently used
                foreach (string saying in pastSaying) {
                    if (saying == sayingList[sayingIndex]) {
                        validSaying = false;
                        break;
                    }
                }
            } while (!validSaying);

            // Fade out current text
            float fadeTime = 1f;
            float elapsedTime = 0f;
            Color startColor = sayingText.color;
            Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

            while (elapsedTime < fadeTime) {
                elapsedTime += Time.deltaTime;
                sayingText.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeTime);
                yield return null;
            }

            // Set new text
            string localizedSayingText = LocalizationSettings.StringDatabase.GetLocalizedString("UITextSaying", sayingList[sayingIndex]);
            sayingText.text = localizedSayingText;
            Enqueue(sayingList[sayingIndex]);

            // Fade in new text
            elapsedTime = 0f;
            startColor = targetColor;
            targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

            while (elapsedTime < fadeTime) {
                elapsedTime += Time.deltaTime;
                sayingText.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeTime);
                yield return null;
            }

            // Wait for 5 seconds before next saying
            yield return new WaitForSeconds(7f);
        }
    }

    private void Enqueue(string saying) {
        if (pastSaying.Count >= maxPastSaying) {
            pastSaying.RemoveFirst(); // Auto-remove oldest item (FIFO)
        }
        pastSaying.AddLast(saying);
    }
}
