using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour {
    [HideInInspector] public CharacterMovementManager characterMovementManager;

    public string charName;

    private float baseHealth = 500;
    private float baseStam = 100;
    private float baseAttack = 50;

    [SerializeField] private int levelPoint = 0;
    [SerializeField] private int healthPoint = 0;
    [SerializeField] private int stamPoint = 0;
    [SerializeField] private int attackPoint = 0;

    private float currentHealth = 500;
    private float currentStam = 100;

    public float totalHealth = 500;
    public float totalStam = 100;
    public float totalAttack = 50;

    public virtual int GetLevelPoint() => levelPoint;
    public virtual void AddLevelPoint(int point) {
        if (point <= 0) return;
        levelPoint += point; 
    }
    public virtual void SpentLevelPoint(string spentTarget) {
        if (levelPoint <= 0) return;

        levelPoint -= 1;
        if (spentTarget == "hp") {
            HealthPoint += 1;
        } else if (spentTarget == "st") {
            StamPoint += 1;
        } else if (spentTarget == "atk") {
            AttackPoint += 1;
        }
    }

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

    public virtual int AttackPoint {
        get => attackPoint;
        set {
            attackPoint = value;
            CalculateTotalAttack();
        }
    }

    protected virtual void Awake() {
        characterMovementManager = GetComponent<CharacterMovementManager>();
    }

    protected virtual void Start() {
        CalculateTotalHealth();
        CalculateTotalStamina();
        CalculateTotalAttack();

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

    public void CalculateTotalAttack() {
        totalAttack = baseAttack + CalculateBonusAttack();
    }

    private float CalculateBonusHealth() {
        return healthPoint * baseHealth * 0.2f;
    }

    private float CalculateBonusStamina() {
        return stamPoint * baseStam * 0.25f;
    }

    private float CalculateBonusAttack() {
        return attackPoint * baseAttack * 0.15f;
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

    public virtual int GetTotalStatsPoint() {
        return levelPoint + healthPoint + stamPoint + attackPoint;
    }

    public virtual void CheckHealthToHandleDeath() {
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
