using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class AlertDeleteSaveSlot : MonoBehaviour {
    private static AlertDeleteSaveSlot instance;
    public static AlertDeleteSaveSlot Instance {
        get => instance;
        private set => instance = value;
    }

    private SaveLoadFileManager saveLoadFileManager;
    [SerializeField] private SaveSlot saveSlot;

    public TextMeshProUGUI alertBoxText;

    public Button confirmDeleteSaveSlot;
    public Button cancelDeleteSaveSlot;

    private string localizedAlertText;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void AttemptToDeleteSaveSlot(SaveSlot slot) {
        localizedAlertText = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "alert_confirm_delete_save");
        alertBoxText.text = slot.ToString().Replace("SaveSlot", localizedAlertText + " ") + "?";
        gameObject.SetActive(true);
        cancelDeleteSaveSlot.Select();
        saveSlot = slot;
    }

    public void ConfirmDeleteSaveSlot() {
        WorldSaveManager.Instance.DeleteGame(saveSlot);
        MenuScreenManager.Instance.OpenLoadGameMenu(false);
        gameObject.SetActive(false);
        MenuScreenManager.Instance.OpenLoadGameMenu(true);
    }

    public void CancelDeleteSaveSlot() {
        gameObject.SetActive(false);
    }
}
