using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFightAndWeapon : MonoBehaviour {
    private static FirstFightAndWeapon instance;
    public static FirstFightAndWeapon Instance {
        get => instance;
        private set => instance = value;
    }

    [SerializeField] private GameObject entryTrigger;
    [SerializeField] private GameObject trapConstruct;
    [SerializeField] private GameObject battleTrigger;


    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        if (SceneData.Instance.hasFirstFightAndWeapon) 
            gameObject.SetActive(false);

        else if (SceneData.Instance.hasStartFirstFightAndWeapon) {
            entryTrigger.SetActive(false);
            trapConstruct.SetActive(true);
            battleTrigger.SetActive(false);
        }
    }

    public void ToggleTrapConstruct() {
        trapConstruct.SetActive(!trapConstruct.activeSelf);
    }
}
