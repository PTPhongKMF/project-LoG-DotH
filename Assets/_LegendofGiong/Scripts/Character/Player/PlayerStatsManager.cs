using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager {
    public override float CurrentHealth {
        get => base.CurrentHealth;
        set {
            PlayerUIManager.Instance.playerUIHudManager.SetNewHealthValue(CurrentHealth, value, totalHealth);
            base.CurrentHealth = value;
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
            PlayerUIManager.Instance.playerUIHudManager.SetNewStamValue(CurrentStam, value, totalStam);
            base.CurrentStam = value;
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
