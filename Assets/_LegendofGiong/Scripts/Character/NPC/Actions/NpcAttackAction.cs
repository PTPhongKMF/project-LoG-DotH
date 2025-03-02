using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/Actions/Attack")]
public class NpcAttackAction : ScriptableObject {
    [SerializeField] private string attackAnimation;

    public NpcAttackAction comboAction;

    [SerializeField] private AttackType attackType;
    // can be repeated?

    public int attackWeight = 50;
    public float actionRecoveryTime = 1.5f;
    public float minimumAttackAngle = -35;
    public float maximumAttackAngle = 35;
    public float minimumAttackDistance = 0;
    public float maximumAttackDistance = 3;

    public void AttemptToPerformAction(NpcCharacterManager npcCharacter) {
            npcCharacter.characterAnimatorManager.PlayTargetAttackActionAnimation(attackAnimation, attackType, AnimationSettings.IsPerformingAction | AnimationSettings.CanRotate
                                                                        | AnimationSettings.ApplyRootMotion | AnimationSettings.IsAttacking | AnimationSettings.IsGrounded);
    }
}
