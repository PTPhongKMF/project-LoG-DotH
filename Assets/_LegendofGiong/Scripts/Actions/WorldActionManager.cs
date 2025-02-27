using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldActionManager : MonoBehaviour {
    private static WorldActionManager instance;
    public static WorldActionManager Instance {
        get => instance;
        private set => instance = value;
    }

    public WeaponItemAction[] weaponItemAction;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        for (int i = 0; i < weaponItemAction.Length; i++) {
            weaponItemAction[i].actionId = i;
        }
    }

    public WeaponItemAction GetWeaponItemAction(int id) {
        for (int i = 0; i < weaponItemAction.Length; i++) {
            if (weaponItemAction[i].actionId == id)

            return weaponItemAction[i];
        }

        return null;
    }
}
