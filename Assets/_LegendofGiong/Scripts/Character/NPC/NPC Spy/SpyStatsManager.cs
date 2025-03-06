using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyStatsManager : CharacterStatsManager {
    private PlayerMovementController player;
    [SerializeField] private WeaponItem sword;

    protected override void Awake() {
        if (SceneData.Instance.hasFirstFightAndWeapon)
            gameObject.SetActive(false);

        base.Awake();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>();
    }

    public override void CheckHealthToHandleDeath() {
        if (CurrentHealth > 0)
            return;

        PlayerUIManager.Instance.playerUIPopupManager.ToggleFirstWeaponPopup();
        FirstFightAndWeapon.Instance.ToggleTrapConstruct();
        PlayerUIManager.Instance.playerUIHudManager.allUtilitySlotsCanvasGroup.alpha = 1;

        player.playerInventoryManager.weaponsInRightHandSlots[1] = Instantiate(sword);
        player.playerEquipmentManager.hasHealingAbility = true;
        player.playerEquipmentManager.hasRageAbility = true;

        player.playerStatsManager.AddLevelPoint(10);

        base.CheckHealthToHandleDeath();
    }

}

