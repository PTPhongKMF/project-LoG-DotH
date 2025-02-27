using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class UISaveSlot : MonoBehaviour {
    SaveLoadFileManager saveLoadFileManager;

    public SaveSlot saveSlot;

    public Button loadSaveSlotButton;
    public Button deleteSaveSlotButton;
    public Image icon;
    public TextMeshProUGUI saveSlotNumber;
    public TextMeshProUGUI level;
    public TextMeshProUGUI timePlayed;
    public TextMeshProUGUI location;

    private string localizedSlotText;

    private void Awake() {
    }

    private void OnEnable() {
        ShowSaveSlot();
    }

    private void ShowSaveSlot() {
        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(WorldSaveManager.Instance.gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = WorldSaveManager.Instance.DecideCharacterSaveFileName(saveSlot);

        localizedSlotText = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "saveslot_slot_text");
        saveSlotNumber.text = saveSlot.ToString().Replace("SaveSlot", localizedSlotText + " ");
        if (saveLoadFileManager.IsSaveFileExists()) {
            level.text = "test succesful";
            timePlayed.text = "test succesful";
            location.text = "test succesful";
        } else {
            loadSaveSlotButton.interactable = false;
            deleteSaveSlotButton.interactable = false;
            level.text = string.Empty;
            timePlayed.text = string.Empty;
            location.text = string.Empty;
        }
    }

    public void LoadSaveSlot() {
        WorldSaveManager.Instance.currentCharSaveSlot = saveSlot;
        WorldSaveManager.Instance.LoadGame();
    }

    public void ShowAlertDeleteSaveSlot() {
        MenuScreenManager.Instance.alertDeleteSaveSlot.AttemptToDeleteSaveSlot(saveSlot);
        //AlertDeleteSaveSlot.Instance.AttemptToDeleteSaveSlot(saveSlot);
    }
}