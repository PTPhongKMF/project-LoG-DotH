using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour {
    [HideInInspector] public CharacterMovementManager characterMovementManager;

    public string charName;

    private float baseHealth = 500;
    private float baseStam = 100;

    [SerializeField] private int healthPoint = 0;
    [SerializeField] private int stamPoint = 0;

    private float currentHealth = 500;
    private float currentStam = 100;

    public float totalHealth = 500;
    public float totalStam = 100;

    public virtual float CurrentHealth {
        get => currentHealth;
        set {
            value = Mathf.Clamp(value, 0, totalHealth);
            currentHealth = value;
            if (!characterMovementManager.isDead) CheckHealthToHandleDeath();
        }
    }
    public virtual int HealthPoint {
        get => healthPoint;
        set {
            healthPoint = value;
            CalculateTotalHealth();
            CurrentHealth = totalHealth;
        }
    }

    private float stamRegenTimer = 0f;
    private float stamRegenTickTimer = 0f;
    public float stamRegenTick = 2f;
    [SerializeField] private float stamRegenDelay = 2f;
    public virtual float CurrentStam {
        get => currentStam;
        set {
            value = Mathf.Clamp(value, 0, totalStam);
            ResetStamRegenTimer(currentStam, value);
            currentStam = value;
        }
    }
    public virtual int StamPoint {
        get => stamPoint;
        set {
            stamPoint = value;
            CalculateTotalStamina();
            CurrentStam = totalStam;
        }
    }

    protected virtual void Awake() {
        characterMovementManager = GetComponent<CharacterMovementManager>();
    }

    protected virtual void Start() {
        CalculateTotalHealth();
        CalculateTotalStamina();

        CurrentHealth = totalHealth;
        CurrentStam = totalStam;
    }

    protected virtual void Update() {
        RegenerateStam();
    }

    public void CalculateTotalHealth() {
        totalHealth = baseHealth + CalculateBonusHealth();
    }

    public void CalculateTotalStamina() {
        totalStam = baseStam + CalculateBonusStamina();
    }

    private float CalculateBonusHealth() {
        return healthPoint * baseHealth * 0.2f;
    }

    private float CalculateBonusStamina() {
        return stamPoint * baseStam * 0.25f;
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

    public void CheckHealthToHandleDeath() {
        if (currentHealth <= 0) {
            StartCoroutine(characterMovementManager.ProcessDeathEvent());
        }
    }

    public virtual void ResetStamRegenTimer(float currentStam, float newStam) {
        if (newStam < currentStam) {
            stamRegenTimer = 0;
        }
    }
}
