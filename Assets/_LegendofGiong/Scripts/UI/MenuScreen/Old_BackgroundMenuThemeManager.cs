using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Old_BackgroundMenuThemeManager : MonoBehaviour {
    public AudioSource audioSource;
    public VideoPlayer introVideo;     // First video (non-looping)
    public VideoPlayer loopVideo;     // Second video (looping)
    public Image fadeOverlay;               // UI Image for fade-in and fade-out effect
    private bool isIntroFade = true;  // Add this at class level
    public float fadeInDuration = 3f;     // Duration for fade-in effect
    public float fadeOutDuration = 2f;    // Duration for fade-out effect

    private void Start() {
        StartCoroutine(PlayAudioSequence());
        StartCoroutine(PlayVideoSequence());
    }

    private IEnumerator PlayAudioSequence() {
        yield return new WaitForSeconds(0.5f); // Wait 1 seconds
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length); // Wait until audio finishes
        yield return new WaitForSeconds(1f); // Wait additional 1 seconds
        StartCoroutine(PlayAudioSequence()); // Restart the loop
    }

    private IEnumerator PlayVideoSequence() {
        introVideo.Prepare();
        loopVideo.Prepare();
        while (!introVideo.isPrepared || !loopVideo.isPrepared) {
            yield return null;
        }

        introVideo.Play();
        introVideo.Pause();

        // Start fade-out of black overlay (reveal video)
        yield return StartCoroutine(FadeImage(fadeOverlay, 1, 0, fadeInDuration));

        // Wait for the video to reach last few seconds before fade out
        float endTime = (float)introVideo.length - fadeInDuration;
        yield return new WaitForSeconds(endTime);

        // Fade in black overlay at end of video
        yield return StartCoroutine(FadeImage(fadeOverlay, 0, 1, fadeOutDuration / 2));

        // Start second looping video
        // fadeOverlay.color = new Color(0, 0, 0, 1); // Keep overlay black before next video starts
        loopVideo.Play();

        // Fade out overlay again for looped video reveal
        yield return StartCoroutine(FadeImage(fadeOverlay, 1, 0, fadeOutDuration / 2));
    }

    private IEnumerator FadeImage(Image img, float startAlpha, float endAlpha, float duration) {
        Color color = img.color;
        float timer = 0f;

        // If this is a fade-in (startAlpha > endAlpha), wait for 1 seconds
        if (isIntroFade) {
            if (startAlpha > endAlpha) {
                yield return new WaitForSeconds(1f);
                duration = 2f; // Only fade for the last 2 second

                // Only play intro video during the first fade
                //if (isIntroFade) {
                introVideo.Play();
                isIntroFade = false;
                //}
            }
        }

        while (timer < duration) {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            img.color = color;
            yield return null;
        }

        color.a = endAlpha;
        img.color = color;
    }
}
