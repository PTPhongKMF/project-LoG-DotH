using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine;

using TMPro;

public class MinigameManager : MonoBehaviour {
    public GameObject resultGameObject;
    public CanvasGroup resultCanvasGroup;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI rewardText;

    public void ShowResultMinigame(string result, int reward = 0) {
        resultText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", result);
        if (result == "you_won") resultText.color = Color.green;
        else resultText.color = Color.red;

        rewardText.text = string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "reward_point"), reward);
        
        resultGameObject.SetActive(true);
        resultCanvasGroup.alpha = 1f; // Make sure it starts fully visible
        
        StartCoroutine(FadeOutResult());
    }

    private IEnumerator FadeOutResult() {
        // Wait 2 seconds before starting fade
        yield return new WaitForSeconds(2f);

        // Fade out over 3 seconds
        float elapsedTime = 0f;
        float startAlpha = resultCanvasGroup.alpha;

        while (elapsedTime < 3f) {
            elapsedTime += Time.deltaTime;
            resultCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / 3f);
            yield return null;
        }

        // Ensure it's fully faded out
        resultCanvasGroup.alpha = 0f;
        resultGameObject.SetActive(false);
    }
}
