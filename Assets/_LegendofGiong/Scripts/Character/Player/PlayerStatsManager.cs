using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager {
    public override float CurrentHealth {
        get => base.CurrentHealth;
        set {
            float oldHealth = base.CurrentHealth;
            base.CurrentHealth = value;
            PlayerUIManager.Instance.playerUIHudManager.SetNewHealthValue(oldHealth, Mathf.Clamp(value, 0, totalHealth));
        }
    }
    public override int HealthPoint {
        get => base.HealthPoint;
        set {
            base.HealthPoint = value;
            PlayerUIManager.Instance.playerUIHudManager.SetMaxHealthValue(totalHealth);
        }
    }

    public override float CurrentStam {
        get => base.CurrentStam;
        set {
            float oldStam = base.CurrentStam;
            base.CurrentStam = value;
            PlayerUIManager.Instance.playerUIHudManager.SetNewStamValue(oldStam, Mathf.Clamp(value, 0, totalStam));
        }
    }
    public override int StamPoint {
        get => base.StamPoint;
        set {
            base.StamPoint = value;
            PlayerUIManager.Instance.playerUIHudManager.SetMaxStamValue(totalStam);
        }
    }

    protected override void Awake() {
        base.Awake();
    }

    protected override void Start() {
        base.Start();

        charName = "Gióng";
        PlayerUIManager.Instance.playerUIHudManager.SetMaxHealthValue(totalHealth);
        PlayerUIManager.Instance.playerUIHudManager.SetMaxStamValue(totalStam);
    }

    protected override void Update() {
        base.Update();
    }
}
