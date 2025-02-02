using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour {
    private float baseStam = 100;

    private float stamPoint = 0;

    private float currentStam = 100;

    public float totalStam = 100;

    private float stamRegenTimer = 0f;
    private float stamRegenTickTimer = 0f;
    public float stamRegenTick = 2f;
    [SerializeField] private float stamRegenDelay = 2f;
    public virtual float CurrentStam {
        get => currentStam;
        set {
            ResetStamRegenTimer(currentStam, value);
            currentStam = value;
        }
    }

    protected virtual void Awake() {
  
    }

    protected virtual void Start() {
        totalStam = CalculateTotalStamina();
        CurrentStam = totalStam;
    }

    protected virtual void Update() {
        RegenerateStam();
    }

    public float CalculateTotalStamina() {
        return baseStam + CalculateBonusStamina();
    }

    private float CalculateBonusStamina() {
        return stamPoint * baseStam * 0.05f;
    }

    public virtual void RegenerateStam() {
        if (PlayerInputController.Instance.moveValue >= 1.5f || 
            PlayerInputController.Instance.playerMovementController.isPerformingAction) return;

        stamRegenTimer += Time.deltaTime;

        if (stamRegenTimer >= stamRegenDelay) {
            if (currentStam < totalStam) {
                stamRegenTickTimer += Time.deltaTime;

                if (stamRegenTickTimer >= 0.1f) {
                    stamRegenTickTimer = 0f;
                    CurrentStam += stamRegenTick;
                }
            }
        }
    }

    public virtual void ResetStamRegenTimer(float currentStam, float newStam) {
        if (newStam < currentStam) {
            stamRegenTimer = 0;
        }
    }
}
