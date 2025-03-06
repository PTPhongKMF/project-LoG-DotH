using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData {
    public string charName;

    public float secondsPlayed;

    public string worldSceneName;
    public string locationName;
    public float xWorldPosition;
    public float yWorldPosition;
    public float zWorldPosition;

    public int levelPoint;
    public int attackPoint;
    public int healthPoint;
    public int stamPoint;

    // Weapon equipment data
    public int currentLeftHandWeaponId;
    public int currentRightHandWeaponId;
    public int[] weaponsInLeftHandSlots = new int[3];
    public int[] weaponsInRightHandSlots = new int[3];
    public int leftHandWeaponIndex;
    public int rightHandWeaponIndex;
    public bool isArmed;
    public bool revertIsArmed;
    public bool hasHealingAbility = false;
    public bool hasRageAbility = false;

    public float allUtilitySlotsCanvasGroup = 0f;

    // mission
    public bool hasStartFirstFightAndWeapon = false;
    public bool hasFirstFightAndWeapon = false;
}
