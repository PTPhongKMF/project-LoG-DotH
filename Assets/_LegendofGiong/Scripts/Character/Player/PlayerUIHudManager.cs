using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour {
    private PlayerMovementController playerMovementController;

    [SerializeField] private StatusBar healthBar;
    [SerializeField] private StatusBar stamBar;

    public CanvasGroup allWeaponSlotsCanvasGroup;
    public CanvasGroup[] weaponSlotCanvasGroup;
    public Image[] weaponSlotIcon;
    public CanvasGroup allUtilitySlotsCanvasGroup;
    public Image[] utilitySlotIcon;

    private void Awake() {
        playerMovementController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    private void Start() {
        RefreshEquipmentSlotsUI();
    }

    private void Update() {
        if (playerMovementController.isArmed) allWeaponSlotsCanvasGroup.alpha = 1.0f;
        else allWeaponSlotsCanvasGroup.alpha = 0f;
    }

    public void SetNewHealthValue(float oldValue, float newValue) {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(float maxHealth) {
        healthBar.SetMaxStat(maxHealth);
    }

    public void SetNewStamValue(float oldValue, float newValue) {
        stamBar.SetStat(newValue);
    }

    public void SetMaxStamValue(float maxStam) {
        stamBar.SetMaxStat(maxStam);
    }

    public void RefreshEquipmentSlotsUI() {
        if (!playerMovementController.isArmed) allWeaponSlotsCanvasGroup.alpha = 0f;
        else allWeaponSlotsCanvasGroup.alpha = 1f;

        for (int i = 0; i <= 2; i++) {
            SetWeaponSlotIcon(playerMovementController.playerInventoryManager.weaponsInRightHandSlots[i].itemId, i);
        }

        RefreshActiveWeaponSlot();
    }

    public void RefreshActiveWeaponSlot() {
        for (int i = 0; i <= 2; i++) {
            if (playerMovementController.playerInventoryManager.rightHandWeaponIndex == i) weaponSlotCanvasGroup[i].alpha = 1.0f;
            else weaponSlotCanvasGroup[i].alpha = 0.2f;
        }
    }

    public void SetWeaponSlotIcon(int weaponId, int iconIndex) {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponById(weaponId);

        if (weapon == null || weapon.itemIcon == null) {
            weaponSlotIcon[iconIndex].enabled = false;
            weaponSlotIcon[iconIndex].sprite = null;
            return;
        }

        weaponSlotIcon[iconIndex].sprite = weapon.itemIcon;
        weaponSlotIcon[iconIndex].enabled = true;
    }
}
