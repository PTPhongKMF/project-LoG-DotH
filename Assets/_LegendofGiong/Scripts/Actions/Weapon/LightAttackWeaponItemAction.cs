using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction {
    [SerializeField] string light_attack_01 = "Righthand_Light_Attack_01";

    public override void AttemptToPerformAction(PlayerMovementController playerPerforming, WeaponItem weaponPerforming) {
        base.AttemptToPerformAction(playerPerforming, weaponPerforming);

        if (!playerPerforming.isGrounded) return;
        if (playerPerforming.isPerformingAction) return;

        PerformLightAttack(playerPerforming, weaponPerforming);
    }

    private void PerformLightAttack(PlayerMovementController playerPerforming, WeaponItem weaponPerforming) {
        if (playerPerforming.isUsingRightHand) {
            playerPerforming.playerAnimatorController.PlayTargetAttackActionAnimation(light_attack_01, AttackType.LightAttack01 , AnimationSettings.IsPerformingAction | AnimationSettings.IsGrounded
                                                                                      | AnimationSettings.CanRotate | AnimationSettings.CanMove | AnimationSettings.IsAttacking);

        }
    }
}
