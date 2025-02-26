using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager {
    private PlayerEquipmentManager playerEquipmentManager;

    public WeaponItem currentLeftHandWeapon;
    public WeaponItem currentRightHandWeapon;
    //private int currentLeftHandWeaponId;
    //private int currentRightHandWeaponId;

    //public int CurrentLeftHandWeaponId { 
    //    get => currentLeftHandWeaponId; 
    //    set {
    //        currentLeftHandWeapon = WorldItemDatabase.Instance.GetWeaponById(value);
    //        playerEquipmentManager.LoadWeaponLeftHand();
    //        currentLeftHandWeaponId = value;
    //    }
    //}
    //public int CurrentRightHandWeaponId { 
    //    get => currentRightHandWeaponId;
    //    set {
    //        currentRightHandWeapon = WorldItemDatabase.Instance.GetWeaponById(value);
    //        playerEquipmentManager.LoadWeaponRightHand();
    //        currentRightHandWeaponId = value;
    //    }
    //}

    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
    public int leftHandWeaponIndex = 0;
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
    public int rightHandWeaponIndex = 0;

    protected override void Awake() {
        base.Awake();

        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
    }
}
