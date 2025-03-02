using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction {
    [SerializeField] string light_attack_01 = "Righthand_Light_Attack_01";
    [SerializeField] string light_attack_02 = "Righthand_Light_Attack_02";
    [SerializeField] string light_attack_03 = "Righthand_Light_Attack_03";

    public override void AttemptToPerformAction(PlayerMovementController playerPerforming, WeaponItem weaponPerforming) {
        base.AttemptToPerformAction(playerPerforming, weaponPerforming);

        if (!playerPerforming.isGrounded) return;

        PerformLightAttack(playerPerforming, weaponPerforming);
    }

    private void PerformLightAttack(PlayerMovementController playerPerforming, WeaponItem weaponPerforming) {
        if (playerPerforming.playerCombatManager.canCombo && playerPerforming.isPerformingAction) {
            playerPerforming.playerCombatManager.canCombo = false;

            if (playerPerforming.playerCombatManager.lastAttackAnimationPerformed == light_attack_01) {
                playerPerforming.playerAnimatorController.PlayTargetAttackActionAnimation(light_attack_02, AttackType.LightAttack02, AnimationSettings.IsPerformingAction | AnimationSettings.IsGrounded
                             | AnimationSettings.CanRotate | AnimationSettings.CanMove | AnimationSettings.IsAttacking);
            } else if (playerPerforming.playerCombatManager.lastAttackAnimationPerformed == light_attack_02) {
                playerPerforming.playerAnimatorController.PlayTargetAttackActionAnimation(light_attack_03, AttackType.LightAttack03, AnimationSettings.IsPerformingAction | AnimationSettings.IsGrounded
                             | AnimationSettings.CanRotate | AnimationSettings.CanMove | AnimationSettings.IsAttacking);
            } else {
                playerPerforming.playerAnimatorController.PlayTargetAttackActionAnimation(light_attack_01, AttackType.LightAttack01, AnimationSettings.IsPerformingAction | AnimationSettings.IsGrounded
                             | AnimationSettings.CanRotate | AnimationSettings.CanMove | AnimationSettings.IsAttacking);
            }
        } else if (!playerPerforming.isPerformingAction) {
            playerPerforming.playerAnimatorController.PlayTargetAttackActionAnimation(light_attack_01, AttackType.LightAttack01, AnimationSettings.IsPerformingAction | AnimationSettings.IsGrounded
                             | AnimationSettings.CanRotate | AnimationSettings.CanMove | AnimationSettings.IsAttacking);
        }
    }
}
