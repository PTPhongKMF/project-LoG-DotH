using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkipCutsceneButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button skipButton;
    [SerializeField] private CanvasGroup skipCanvasGroup;
    
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        skipButton = GetComponent<Button>();
        skipCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(InitialFadeSequence());
    }

    private IEnumerator InitialFadeSequence()
    {
        skipCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(2f); // Wait for 2 seconds at full alpha
        yield return StartFade(0f, 2f); // Fade to 0 over 2 seconds
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        
        skipCanvasGroup.alpha = 1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        
        fadeCoroutine = StartCoroutine(StartFade(0f, 2f));
    }

    private IEnumerator StartFade(float targetAlpha, float duration)
    {
        float startAlpha = skipCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            skipCanvasGroup.alpha = currentAlpha;
            yield return null;
        }

        skipCanvasGroup.alpha = targetAlpha;
    }

    public void SkipToNextScene(string sceneName) {
        StartCoroutine(WorldSaveManager.Instance.LoadWorldScene(sceneName));
    }
}
