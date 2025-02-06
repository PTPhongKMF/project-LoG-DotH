using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class SaveLoadFileManager {
    public string saveFileDirPath = string.Empty;
    public string saveFileName = string.Empty;

    public bool IsSaveFileExists() {
        if (File.Exists(Path.Combine(saveFileDirPath, saveFileName))) {
            return true;
        } else {
            return false;
        }
    }

    public void DeleteSaveFile() {
        File.Delete(Path.Combine(saveFileDirPath, saveFileName));
    }

    public void CreateSaveFile(CharacterSaveData charData) {
        string savePath = Path.Combine(saveFileDirPath, saveFileName);

        try {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            string dataToStore = JsonUtility.ToJson(charData, true);

            using (FileStream fileStream = new FileStream(savePath, FileMode.Create)) {
                using (StreamWriter fileWriter = new StreamWriter(fileStream)) {
                    fileWriter.Write(dataToStore);
                }
            }
        } catch (Exception e) {
            Debug.LogError("Error while trying to save data. PATH: " + savePath + "\n" + e);
        }
    }

    public CharacterSaveData LoadSaveFile() {
        CharacterSaveData charData = null;
        string loadPath = Path.Combine(saveFileDirPath, saveFileName);

        if (File.Exists(loadPath)) {
            try {
                string dataToLoad = string.Empty;

                using (FileStream fileStream = new FileStream(loadPath, FileMode.Open)) {
                    using (StreamReader fileReader = new StreamReader(fileStream)) {
                        dataToLoad = fileReader.ReadToEnd();
                    }
                }
                charData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            } catch (Exception e) {
                Debug.LogError(e);
            }
        }

        return charData;
    }
}
