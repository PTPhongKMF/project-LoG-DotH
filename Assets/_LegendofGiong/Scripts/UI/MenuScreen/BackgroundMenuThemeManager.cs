using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BackgroundMenuThemeManager : MonoBehaviour {
    public AudioSource audioSource;
    public VideoPlayer loopVideo;     // Video player for looping video
    public Image fadeOverlay;         // UI Image for fade-in effect
    public float fadeInDuration = 3f; // Duration for fade-in effect

    private void Start() {
        StartCoroutine(PlayAudioSequence());
        StartCoroutine(PlayVideoSequence());
    }

    private IEnumerator PlayAudioSequence() {
        yield return new WaitForSeconds(0.5f);
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayAudioSequence());
    }

    private IEnumerator PlayVideoSequence() {
        loopVideo.Prepare();
        while (!loopVideo.isPrepared) {
            yield return null;
        }

        // Set the video to loop
        loopVideo.isLooping = true;
        
        // Start playing the video
        loopVideo.Play();

        // Fade in (from black to transparent)
        yield return StartCoroutine(FadeImage(fadeOverlay, 1, 0, fadeInDuration));
    }

    private IEnumerator FadeImage(Image img, float startAlpha, float endAlpha, float duration) {
        Color color = img.color;
        float timer = 0f;

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
