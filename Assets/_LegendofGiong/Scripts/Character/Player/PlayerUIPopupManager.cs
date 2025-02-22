using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIPopupManager : MonoBehaviour {
    [SerializeField] private GameObject youDiedPopup;
    [SerializeField] private CanvasGroup youDiedPopupCanvasGroup; // set alpha to fade overtime
    [SerializeField] private TextMeshProUGUI youDiedPopupBackgroundText;
    [SerializeField] private TextMeshProUGUI youDiedPopupText;

    public void ShowYouDiedPopup() {
        youDiedPopup.SetActive(true);
        youDiedPopupBackgroundText.characterSpacing = 0;

        StartCoroutine(StretchPopupTextOvertime(youDiedPopupBackgroundText, 8, 19f));
        StartCoroutine(FadeInPopupOvertime(youDiedPopupCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutPopupOvertime(youDiedPopupCanvasGroup, 2, 5));
    }

    private IEnumerator StretchPopupTextOvertime(TextMeshProUGUI text, float duration, float stretchAmount) {
        if (duration > 0) {
            text.characterSpacing = 0;
            float timer = 0;
            yield return null;

            while (timer < duration) {
                timer += Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopupOvertime(CanvasGroup canvas, float duration) {
        if (duration > 0) {
            canvas.alpha = 0;
            float timer = 0;
            yield return null;

            while (timer < duration) {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 1;
        yield return null;
    }

    private IEnumerator WaitThenFadeOutPopupOvertime(CanvasGroup canvas, float duration, float delay) {
        if (duration > 0) {
            while (delay > 0) {
                delay -= Time.deltaTime;
                yield return null;
            }

            canvas.alpha = 1;
            float timer = 0;
            yield return null;

            while (timer < duration) {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 0;
        yield return null;
    }

}
