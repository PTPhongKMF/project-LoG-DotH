using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UISaveSlot : MonoBehaviour {
    SaveLoadFileManager saveLoadFileManager;

    public SaveSlot saveSlot;

    private Button saveSlotButton;
    public Image icon;
    public TextMeshProUGUI saveSlotNumber;
    public TextMeshProUGUI level;
    public TextMeshProUGUI timePlayed;
    public TextMeshProUGUI location;

    private void OnEnable() {
        saveSlotButton = GetComponent<Button>();

        ShowSaveSlot();
    }

    private void ShowSaveSlot() {
        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(WorldSaveManager.Instance.gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = WorldSaveManager.Instance.DecideCharacterSaveFileName(saveSlot);

        saveSlotNumber.text = saveSlot.ToString().Replace("SaveSlot", "Slot ");
        if (saveLoadFileManager.IsSaveFileExists()) {
            level.text = "test succesful";
            timePlayed.text = "test succesful";
            location.text = "test succesful";
        } else {
            saveSlotButton.interactable = false;
            level.text = string.Empty;
            timePlayed.text = string.Empty;
            location.text = string.Empty;
        }
    }

    public void LoadSaveSlot() {
        WorldSaveManager.Instance.currentCharSaveSlot = saveSlot;
        WorldSaveManager.Instance.LoadGame();
    }
}