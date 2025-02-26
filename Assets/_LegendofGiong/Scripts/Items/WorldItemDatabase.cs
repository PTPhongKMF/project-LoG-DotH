using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour {
    private static WorldItemDatabase instance;
    public static WorldItemDatabase Instance {
        get => instance;
        private set => instance = value;
    }

    public WeaponItem unarmedWeapon; 

    [SerializeField] private List<WeaponItem> weapons = new List<WeaponItem>();

    // list of every items we have in the game
    private List<Item> items = new List<Item>();

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        foreach (var weapon in weapons) {
            items.Add(weapon);
        }

        for (int i = 0; i < items.Count; i++) {
            items[i].itemId = i; 
        }
    }

    //public WeaponItem GetWeaponById(int id) {
    //    for (int i = 0; i < weapons.Count; i++) {
    //        if (weapons[i].itemId == id) {
    //            return weapons[i];
    //        }
    //    }

    //    return null;
    //}
}
