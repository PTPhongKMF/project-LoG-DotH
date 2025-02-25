using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager {
    private PlayerMovementController playerController;

    public WeaponModelSpawnSlot leftHandSlot;
    public WeaponModelSpawnSlot rightHandSlot;

    public GameObject leftHandWeaponModel;
    public GameObject rightHandWeaponModel;

    protected override void Awake() {
        base.Awake();

        playerController = GetComponent<PlayerMovementController>();

        InitializeWeaponSlot();
    }

    protected override void Start() {
        base.Start();

        LoadWeaponOnBothHands();
    }

    private void InitializeWeaponSlot() {
        WeaponModelSpawnSlot[] weaponAttachSlots = GetComponentsInChildren<WeaponModelSpawnSlot>();

        foreach (var weaponSlot in weaponAttachSlots) {
            switch (weaponSlot.weaponAttachSlot) {
                case CharacterItemAttachSlot.Lefthand:
                    leftHandSlot = weaponSlot;
                    break;
                case CharacterItemAttachSlot.Righthand:
                    rightHandSlot = weaponSlot;
                    break;
            }
        }
    }

    public void LoadWeaponOnBothHands() {
        LoadWeaponLeftHand();
        LoadWeaponRightHand();
    }

    public void LoadWeaponLeftHand() {
        if (playerController.playerInventoryManager.currentLeftHandWeapon != null) {
            leftHandWeaponModel = Instantiate(playerController.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
        }
    }

    public void LoadWeaponRightHand() {
        if (playerController.playerInventoryManager.currentRightHandWeapon != null) {
            rightHandWeaponModel = Instantiate(playerController.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
        }
    }
}
