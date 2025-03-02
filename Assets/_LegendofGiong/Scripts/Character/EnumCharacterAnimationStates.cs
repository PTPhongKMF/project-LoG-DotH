using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumCharacterAnimationStates : MonoBehaviour {
}

[Flags]
public enum AnimationSettings {
    None = 0,                  
    IsPerformingAction = 1 << 0,
    CanMove = 1 << 1,          
    CanRotate = 1 << 2,        
    CanDodge = 1 << 3,         
    CanJump = 1 << 4,          
    IsJumping = 1 << 5,        
    IsGrounded = 1 << 6,
    IsAttacking = 1 << 7,
    ApplyRootMotion = 1 << 8,  
    RevertApplyRootMotion = 1 << 9,
}
