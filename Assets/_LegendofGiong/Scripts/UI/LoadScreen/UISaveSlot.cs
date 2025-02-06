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

    public Image icon;
    public TextMeshProUGUI saveSlotNumber;
    public TextMeshProUGUI level;
    public TextMeshProUGUI timePlayed;
    public TextMeshProUGUI location;

    private void OnEnable() {
        LoadSaveSlot();
    }

    private void LoadSaveSlot() {
        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(WorldManager.Instance.gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = WorldManager.Instance.DecideCharacterSaveFileName(saveSlot);

        saveSlotNumber.text = saveSlot.ToString().Replace("SaveSlot", "Slot ");
        if (saveLoadFileManager.IsSaveFileExists()) {
            level.text = "test succesful";
            timePlayed.text = "test succesful";
            location.text = "test succesful";
        } else {
            level.text = string.Empty;
            timePlayed.text = string.Empty;
            location.text = string.Empty;
        }
    }
}