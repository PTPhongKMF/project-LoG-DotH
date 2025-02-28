using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager {
    private PlayerMovementController playerController;

    public WeaponModelSpawnSlot leftHandSlot;
    public WeaponModelSpawnSlot rightHandSlot;

    [SerializeField] private WeaponManager leftWeaponManager;
    [SerializeField] private WeaponManager rightWeaponManager;

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

    public void UnloadWeaponOnBothHands() {
        leftHandSlot.UnloadWeapon();
        rightHandSlot.UnloadWeapon();
    }

    //  LEFT HAND WEAPON

    public void LoadWeaponLeftHand() {
        if (playerController.playerInventoryManager.currentLeftHandWeapon != null) {
            leftHandSlot.UnloadWeapon();
            leftHandWeaponModel = Instantiate(playerController.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(playerController, playerController.playerInventoryManager.currentLeftHandWeapon);
        }
    }

    public void SwitchLeftWeaponToIndex(int index) {
        playerController.playerInventoryManager.currentLeftHandWeapon = Instantiate(
            playerController.playerInventoryManager.weaponsInLeftHandSlots[index]);
        playerController.playerInventoryManager.leftHandWeaponIndex = index;

        LoadWeaponLeftHand();
    }

    public void SwitchLeftWeapon() {
        // play switch animation here if you have
        //playerController.playerAnimatorController.PlayTargetActionAnimation();

        WeaponItem selectedWeapon = null;
        playerController.playerInventoryManager.leftHandWeaponIndex += 1;
        if (playerController.playerInventoryManager.leftHandWeaponIndex < 0 ||
                playerController.playerInventoryManager.leftHandWeaponIndex > 2) {
            playerController.playerInventoryManager.leftHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < playerController.playerInventoryManager.weaponsInLeftHandSlots.Length; i++) {
                if (playerController.playerInventoryManager.weaponsInLeftHandSlots[i].itemId != WorldItemDatabase.Instance.unarmedWeapon.itemId) {
                    weaponCount += 1;

                    if (firstWeapon == null) {
                        firstWeapon = playerController.playerInventoryManager.weaponsInLeftHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1) {
                playerController.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                playerController.playerInventoryManager.currentLeftHandWeapon = Instantiate(selectedWeapon);
            } else {
                playerController.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                playerController.playerInventoryManager.currentLeftHandWeapon = firstWeapon;
            }

            LoadWeaponLeftHand();
            return;
        }

        //foreach (WeaponItem item in playerController.playerInventoryManager.weaponsInLeftHandSlots) {
        if (playerController.playerInventoryManager.weaponsInLeftHandSlots
            [playerController.playerInventoryManager.leftHandWeaponIndex].itemId !=
            WorldItemDatabase.Instance.unarmedWeapon.itemId) {
            selectedWeapon = playerController.playerInventoryManager
                .weaponsInLeftHandSlots[playerController.playerInventoryManager.leftHandWeaponIndex];
            playerController.playerInventoryManager.currentLeftHandWeapon = Instantiate(selectedWeapon);
            LoadWeaponLeftHand();
            return;
        }
        //}

        if (selectedWeapon == null && playerController.playerInventoryManager.rightHandWeaponIndex <= 2) {
            SwitchLeftWeapon();
        }
    }

    //  RIGHT HAND WEAPON

    public void LoadWeaponRightHand() {
        if (playerController.playerInventoryManager.currentRightHandWeapon != null) {
            rightHandSlot.UnloadWeapon();
            rightHandWeaponModel = Instantiate(playerController.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(playerController, playerController.playerInventoryManager.currentRightHandWeapon);
        }
    }

    public void SwitchRightWeaponToIndex(int index) {
        playerController.playerInventoryManager.currentRightHandWeapon = Instantiate(
            playerController.playerInventoryManager.weaponsInRightHandSlots[index]);
        playerController.playerInventoryManager.rightHandWeaponIndex = index;
        PlayerUIManager.Instance.playerUIHudManager.RefreshActiveWeaponSlot(); 

        LoadWeaponRightHand();
    }

    public void SwitchRightWeapon() {
        // play switch animation here if you have
        //playerController.playerAnimatorController.PlayTargetActionAnimation();

        WeaponItem selectedWeapon = null;
        playerController.playerInventoryManager.rightHandWeaponIndex += 1;
        if (playerController.playerInventoryManager.rightHandWeaponIndex < 0 ||
                playerController.playerInventoryManager.rightHandWeaponIndex > 2) {
            playerController.playerInventoryManager.rightHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < playerController.playerInventoryManager.weaponsInRightHandSlots.Length; i++) {
                if (playerController.playerInventoryManager.weaponsInRightHandSlots[i].itemId != WorldItemDatabase.Instance.unarmedWeapon.itemId) {
                    weaponCount += 1;

                    if (firstWeapon == null) {
                        firstWeapon = playerController.playerInventoryManager.weaponsInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1) {
                playerController.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                playerController.playerInventoryManager.currentRightHandWeapon = Instantiate(selectedWeapon);
            } else {
                playerController.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                playerController.playerInventoryManager.currentRightHandWeapon = firstWeapon;
            }

            LoadWeaponRightHand();
            return;
        }

        //foreach (WeaponItem item in playerController.playerInventoryManager.weaponsInRightHandSlots) {
        if (playerController.playerInventoryManager.weaponsInRightHandSlots
            [playerController.playerInventoryManager.rightHandWeaponIndex].itemId !=
            WorldItemDatabase.Instance.unarmedWeapon.itemId) {
            selectedWeapon = playerController.playerInventoryManager
                .weaponsInRightHandSlots[playerController.playerInventoryManager.rightHandWeaponIndex];
            playerController.playerInventoryManager.currentRightHandWeapon = Instantiate(selectedWeapon);
            LoadWeaponRightHand();
            return;
        }
        //}

        if (selectedWeapon == null && playerController.playerInventoryManager.rightHandWeaponIndex <= 2) {
            SwitchRightWeapon();
        }
    }

    // DAMAGE COLLIDER

    public void OpenDamageCollider() {
        if (playerController.isUsingRightHand) {
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
        } else if (playerController.isUsingLeftHand) {
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }
    }

    public void CloseDamageCollider() {
        if (playerController.isUsingRightHand) {
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        } else if (playerController.isUsingLeftHand) {
            leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
    }
}
