using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonCheck : MonoBehaviour {
    private Button saveButton;

    private void Awake() {
        saveButton = GetComponent<Button>();
    }

    private void OnEnable() {
        if (SceneData.Instance.isSafeToSave)
            saveButton.interactable = true;
        else
            saveButton.interactable = false;
    }
}
