using System;
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

    public bool hasHealingAbility = false;
    public bool hasRageAbility = false;

    private bool isHealingOnCooldown = false;
    private float healingCooldownTimer = 0f;
    private float healOverTimeTimer = 0f;
    private float healOverTimeAmount = 0f;
    private bool isHealOverTimeActive = false;

    private bool isRageOnCooldown = false;
    private float rageCooldownTimer = 0f;
    private bool isRageActive = false;
    private float rageTimer = 0f;
    private int currentRageBonusPoints = 0;

    protected override void Awake() {
        base.Awake();

        playerController = GetComponent<PlayerMovementController>();

        InitializeWeaponSlot();
    }

    protected override void Start() {
        base.Start();

        LoadWeaponOnBothHands();
    }

    protected override void Update() {
        base.Update();
        HandleHealingCooldownAndHoT();
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
        int index = playerController.playerInventoryManager.rightHandWeaponIndex;
        if (playerController.playerInventoryManager.weaponsInRightHandSlots[index].weaponType != WeaponType.Fist) {
            playerController.playerAnimatorController.PlayTargetActionAnimation("draw_sword", AnimationSettings.CanMove | AnimationSettings.CanJump | AnimationSettings.IsGrounded | AnimationSettings.CanDodge | AnimationSettings.CanRotate);
            playerController.playerSoundFXController.PlayDrawBladeSFX();
        }

        LoadWeaponLeftHand();
        LoadWeaponRightHand();
    }

    public void UnloadWeaponOnBothHands() {
        int index = playerController.playerInventoryManager.rightHandWeaponIndex;
        if (playerController.playerInventoryManager.weaponsInRightHandSlots[index].weaponType != WeaponType.Fist) {
            playerController.playerAnimatorController.PlayTargetActionAnimation("sheath_sword", AnimationSettings.CanMove | AnimationSettings.CanJump | AnimationSettings.IsGrounded | AnimationSettings.CanDodge | AnimationSettings.CanRotate);
            playerController.playerSoundFXController.PlayDrawBladeSFX();
        }

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
        if (!playerController.isArmed && !playerController.revertIsArmed)
            return;

        if (playerController.playerInventoryManager.rightHandWeaponIndex == index)
            return;

         if (playerController.playerInventoryManager.weaponsInRightHandSlots[index].weaponType == WeaponType.Fist &&
            playerController.playerInventoryManager.currentRightHandWeapon.weaponType != WeaponType.Fist) {
            playerController.playerAnimatorController.PlayTargetActionAnimation("sheath_sword", AnimationSettings.CanMove | AnimationSettings.CanJump | AnimationSettings.IsGrounded | AnimationSettings.CanDodge | AnimationSettings.CanRotate);
            playerController.playerSoundFXController.PlayDrawBladeSFX();
        } else if (playerController.playerInventoryManager.weaponsInRightHandSlots[index].weaponType != WeaponType.Fist ||
            playerController.playerInventoryManager.currentRightHandWeapon.weaponType != WeaponType.Fist) {
            playerController.playerAnimatorController.PlayTargetActionAnimation("draw_sword", AnimationSettings.CanMove | AnimationSettings.CanJump | AnimationSettings.IsGrounded | AnimationSettings.CanDodge | AnimationSettings.CanRotate);
            playerController.playerSoundFXController.PlayDrawBladeSFX();
        }

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

    public void TriggerHealingAbility() {
        if (!hasHealingAbility || isHealingOnCooldown) return;

        // Start cooldown
        isHealingOnCooldown = true;
        healingCooldownTimer = 30f; // 30 second cooldown

        // Calculate heal amounts
        float maxHealth = playerController.characterStatsManager.totalHealth;
        float instantHealAmount = maxHealth * 0.25f; // 25% instant heal
        healOverTimeAmount = maxHealth * 0.25f; // 25% heal over time

        // Apply instant heal
        playerController.characterStatsManager.CurrentHealth += instantHealAmount;

        // Setup heal over time
        isHealOverTimeActive = true;
        healOverTimeTimer = 10f; // 10 second duration

        // Update UI
        PlayerUIManager.Instance.playerUIHudManager.OnHealingAbilityTriggered();
    }

    public void TriggerRageAbility() {
        if (!hasRageAbility || isRageOnCooldown) return;

        float randomValue = UnityEngine.Random.value * 100f; // 0-100
        int bonusAttackPoints;

        if (randomValue <= 80f) { // 80% chance for 5-10 points
            bonusAttackPoints = UnityEngine.Random.Range(5, 11);
        }
        else if (randomValue <= 95f) { // 15% chance for 11-80 points
            bonusAttackPoints = UnityEngine.Random.Range(11, 81);
        }
        else { // 5% chance for 81-100 points
            bonusAttackPoints = UnityEngine.Random.Range(81, 101);
        }

        // Start cooldown and duration
        isRageOnCooldown = true;
        rageCooldownTimer = 40f; // 40 second cooldown
        isRageActive = true;
        rageTimer = 20f; // 20 second duration
        currentRageBonusPoints = bonusAttackPoints;

        // Apply bonus
        playerController.characterStatsManager.AttackPoint += bonusAttackPoints;

        // Update UI
        PlayerUIManager.Instance.playerUIHudManager.OnRageAbilityTriggered();
    }

    private void HandleHealingCooldownAndHoT() {
        // Handle cooldown
        if (isHealingOnCooldown) {
            healingCooldownTimer -= Time.deltaTime;
            if (healingCooldownTimer <= 0) {
                isHealingOnCooldown = false;
            }
        }

        // Handle heal over time
        if (isHealOverTimeActive) {
            healOverTimeTimer -= Time.deltaTime;
            
            float healPerSecond = healOverTimeAmount / 10f; // Total heal divided by 10 seconds
            playerController.characterStatsManager.CurrentHealth += healPerSecond * Time.deltaTime;

            if (healOverTimeTimer <= 0) {
                isHealOverTimeActive = false;
            }
        }

        // Handle rage duration and cooldown
        if (isRageActive) {
            rageTimer -= Time.deltaTime;
            if (rageTimer <= 0) {
                isRageActive = false;
                // Remove the bonus when rage ends
                playerController.characterStatsManager.AttackPoint -= currentRageBonusPoints;
                currentRageBonusPoints = 0;
            }
        }

        if (isRageOnCooldown) {
            rageCooldownTimer -= Time.deltaTime;
            if (rageCooldownTimer <= 0) {
                isRageOnCooldown = false;
            }
        }
    }
}
