using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour {
    private PlayerMovementController playerMovementController;

    [SerializeField] CanvasGroup[] canvasGroup;

    [SerializeField] private StatusBar healthBar;
    [SerializeField] private StatusBar stamBar;

    public CanvasGroup allWeaponSlotsCanvasGroup;
    public CanvasGroup[] weaponSlotCanvasGroup;
    public Image[] weaponSlotIcon;
    public CanvasGroup allUtilitySlotsCanvasGroup;
    public Image[] utilitySlotIcon;
    public Image[] utilitySlotIconOverlay;
    public TextMeshProUGUI[] utilitySlotCooldown;

    private float healingCooldownDuration = 30f;
    private float healingHoTDuration = 10f;
    private float rageDuration = 20f;
    private float rageCooldownDuration = 40f;

    private bool isHealingOnCooldown = false;
    private float healingCooldownTimer = 0f;
    private bool isHealingHoTActive = false;
    private float healingHoTTimer = 0f;

    private bool isRageOnCooldown = false;
    private float rageCooldownTimer = 0f;
    private bool isRageActive = false;
    private float rageTimer = 0f;

    private void Awake() {
        playerMovementController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    private void Start() {
        RefreshEquipmentSlotsUI();
        // Initialize utility slots UI
        ResetUtilitySlotUI(0); // Healing slot
        ResetUtilitySlotUI(1); // Rage slot
    }

    private void Update() {
        if (playerMovementController.isArmed || playerMovementController.revertIsArmed) allWeaponSlotsCanvasGroup.alpha = 1.0f;
        else allWeaponSlotsCanvasGroup.alpha = 0f;

        UpdateHealingAbilityUI();
        UpdateRageAbilityUI();
    }

    private void UpdateHealingAbilityUI() {
        if (isHealingOnCooldown) {
            // Update cooldown overlay
            float cooldownProgress = healingCooldownTimer / healingCooldownDuration;
            utilitySlotIconOverlay[0].rectTransform.sizeDelta = new Vector2(
                utilitySlotIconOverlay[0].rectTransform.sizeDelta.x,
                100f * cooldownProgress
            );

            // Update cooldown text
            utilitySlotCooldown[0].alpha = 1f;
            utilitySlotCooldown[0].text = Mathf.CeilToInt(healingCooldownTimer).ToString();

            healingCooldownTimer -= Time.deltaTime;
            if (healingCooldownTimer <= 0) {
                isHealingOnCooldown = false;
                ResetUtilitySlotUI(0);
            }
        }

        if (isHealingHoTActive) {
            healingHoTTimer -= Time.deltaTime;
            if (healingHoTTimer <= 0) {
                isHealingHoTActive = false;
            }
        }
    }

    private void UpdateRageAbilityUI() {
        if (isRageActive) {
            // Update rage effect overlay
            float rageProgress = rageTimer / rageDuration;
            utilitySlotIconOverlay[1].rectTransform.sizeDelta = new Vector2(
                utilitySlotIconOverlay[1].rectTransform.sizeDelta.x,
                100f * rageProgress
            );

            // Update duration text
            utilitySlotCooldown[1].alpha = 1f;
            utilitySlotCooldown[1].text = Mathf.CeilToInt(rageTimer).ToString();

            rageTimer -= Time.deltaTime;
            if (rageTimer <= 0) {
                isRageActive = false;
            }
        }

        if (isRageOnCooldown) {
            // Update cooldown overlay
            float cooldownProgress = rageCooldownTimer / rageCooldownDuration;
            utilitySlotIconOverlay[1].rectTransform.sizeDelta = new Vector2(
                utilitySlotIconOverlay[1].rectTransform.sizeDelta.x,
                100f * cooldownProgress
            );

            // Update cooldown text
            utilitySlotCooldown[1].alpha = 1f;
            utilitySlotCooldown[1].text = Mathf.CeilToInt(rageCooldownTimer).ToString();

            rageCooldownTimer -= Time.deltaTime;
            if (rageCooldownTimer <= 0) {
                isRageOnCooldown = false;
                ResetUtilitySlotUI(1);
            }
        }
    }

    private void ResetUtilitySlotUI(int slotIndex) {
        utilitySlotIconOverlay[slotIndex].rectTransform.sizeDelta = new Vector2(
            utilitySlotIconOverlay[slotIndex].rectTransform.sizeDelta.x,
            0f
        );
        utilitySlotCooldown[slotIndex].alpha = 0f;
        utilitySlotCooldown[slotIndex].text = "";
    }

    public void OnHealingAbilityTriggered() {
        isHealingOnCooldown = true;
        healingCooldownTimer = healingCooldownDuration;
        isHealingHoTActive = true;
        healingHoTTimer = healingHoTDuration;
        
        utilitySlotIconOverlay[0].rectTransform.sizeDelta = new Vector2(
            utilitySlotIconOverlay[0].rectTransform.sizeDelta.x,
            100f
        );
    }

    public void OnRageAbilityTriggered() {
        isRageActive = true;
        rageTimer = rageDuration;
        isRageOnCooldown = true;
        rageCooldownTimer = rageCooldownDuration;
        
        utilitySlotIconOverlay[1].rectTransform.sizeDelta = new Vector2(
            utilitySlotIconOverlay[1].rectTransform.sizeDelta.x,
            100f
        );
    }

    //public void ToggleHUD(bool status) {
    //    if (status) {
    //        foreach (CanvasGroup canvas in canvasGroup) {
    //            canvas.alpha = 1f;
    //        }
    //    } else {
    //        foreach (CanvasGroup canvas in canvasGroup) {
    //            canvas.alpha = 0f;
    //        }
    //    }
    //}

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
