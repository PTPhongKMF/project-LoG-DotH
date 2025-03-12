using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class PlayerUIPopupManager : MonoBehaviour {
    [SerializeField] private GameObject youDiedPopup;
    [SerializeField] private CanvasGroup youDiedPopupCanvasGroup; // set alpha to fade overtime
    [SerializeField] private TextMeshProUGUI youDiedPopupBackgroundText;
    [SerializeField] private TextMeshProUGUI youDiedPopupText;

    public GameObject MenuOptions;
    [SerializeField] private GameObject confirmSavedPopup;
    [SerializeField] private CanvasGroup confirmSavedPopupCanvasGroup;

    [SerializeField] private GameObject popupMessageGameObject;
    [SerializeField] private TextMeshProUGUI popupMessageText;

    [SerializeField] private GameObject firstFightPopup;
    [SerializeField] private GameObject firstWeaponPopup;
    [SerializeField] private Button firstFightPopupCloseButton;
    [SerializeField] private Button firstWeaponPopupCloseButton;

    [SerializeField] private GameObject toWarPopup;

    public void SendPlayerMessagePopup(string messageText) {
        string localizedMessageText = LocalizationSettings.StringDatabase.GetLocalizedString("InteractionText", messageText);
        PlayerUIManager.Instance.popupWindowIsOpen = true;
        popupMessageText.text = localizedMessageText;
        popupMessageGameObject.SetActive(true);
    }

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

    public void CloseAllPopupWindows() {
        popupMessageGameObject.SetActive(false);
        PlayerUIManager.Instance.popupWindowIsOpen = false;
    }

    public void SaveGameFromOptionMenu() {
        if (!SceneData.Instance.isSafeToSave)
            return;

        WorldSaveManager.Instance.SaveGame();
        confirmSavedPopup.SetActive(true);
        StartCoroutine(FadeInPopupOvertime(confirmSavedPopupCanvasGroup, 0.5f));
        StartCoroutine(WaitThenFadeOutPopupOvertime(confirmSavedPopupCanvasGroup, 2f, 2f));
    }

    public void CloseConfirmSavedPopup() {
        confirmSavedPopup.SetActive(false);
        confirmSavedPopupCanvasGroup.alpha = 0;
    }

    public void LoadGameFromOptionMenu() {

    }

    public void BackToMenu() {
        StartCoroutine(WorldSaveManager.Instance.LoadWorldScene("MenuScreenScene"));
    }

    public void QuitGame() {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode(); // Stops Play Mode in Unity Editor
#else
            Application.Quit(); // Closes the game in a built application
#endif
    }

    public void ToggleFirstWeaponPopup() {
        if (!firstWeaponPopup.activeSelf) {
            firstWeaponPopup.SetActive(true);
            firstFightPopupCloseButton.interactable = false;
            PlayerInputController.Instance.SetDialogState(true);
            StartCoroutine(EnableCloseFWButtonAfterDelay(3f));
        } else {
            firstWeaponPopup.SetActive(false);
            PlayerInputController.Instance.SetDialogState(false);
        }
    }

    public void ToggleFirstFightPopup() {
        if (!firstFightPopup.activeSelf) {
            firstFightPopup.SetActive(true);
            firstFightPopupCloseButton.interactable = false;
            PlayerInputController.Instance.SetDialogState(true);
            StartCoroutine(EnableCloseFFButtonAfterDelay(3f));
        } else {
            firstFightPopup.SetActive(false);
            PlayerInputController.Instance.SetDialogState(false);
        }
    }

    private IEnumerator EnableCloseFFButtonAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        firstFightPopupCloseButton.interactable = true;
    }

    private IEnumerator EnableCloseFWButtonAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        firstWeaponPopupCloseButton.interactable = true;
    }

    public void ToggleToWarPopup() {
        if (!toWarPopup.activeSelf) {
            toWarPopup.SetActive(true);
            PlayerInputController.Instance.SetDialogState(true);
        } else {
            toWarPopup.SetActive(false);
            PlayerInputController.Instance.SetDialogState(false);
        }
    }

    public void AcceptToWar() {
        toWarPopup.SetActive(false);
        PlayerInputController.Instance.SetDialogState(false);
        //Destroy(PlayerUIManager.Instance.playerUIPopupManager.firstWeaponPopup);
        StartCoroutine(WorldSaveManager.Instance.LoadWorldScene("Warzone"));
    }

}
