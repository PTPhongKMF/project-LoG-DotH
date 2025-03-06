using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUILoadingScreen : MonoBehaviour {
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private CanvasGroup canvasGroup;

    private Coroutine fadeLoadingScreenCoroutine;

    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        DeactivateLoadingScreen();
    }

    public void ActivateLoadingScreen() {
        if (loadingScreen.activeSelf)
            return;

        canvasGroup.alpha = 1.0f;
        loadingScreen.SetActive(true);
    }

    public void DeactivateLoadingScreen(float delay = 3) {
        if (!loadingScreen.activeSelf)
            return;

        if (fadeLoadingScreenCoroutine != null)
            return;

        fadeLoadingScreenCoroutine = StartCoroutine(FadeLoadingScreen(1, delay));
    }

    private IEnumerator FadeLoadingScreen(float duration, float delay) {
        while (PlayerUIManager.Instance.isPerformingLoadingOperation)
            yield return null;

        loadingScreen.SetActive(true);

        if (duration > 0) {
            while (delay > 0) {
                delay -= Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1.0f;
            float elapsedTime = 0;

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
                yield return null;
            }
        }

        canvasGroup.alpha = 0f;
        loadingScreen.SetActive(false);
        fadeLoadingScreenCoroutine = null;
        yield return null;
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
