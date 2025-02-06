using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager {

    public override float CurrentStam {
        get => base.CurrentStam;
        set {
            PlayerUIManager.Instance.playerUIHudManager.SetNewStamValue(CurrentStam, value);
            base.CurrentStam = value;
        }
    }

    protected override void Awake() {
        base.Awake();
    }

    protected override void Start() {
        base.Start();

        charName = "Gi�ng";
        PlayerUIManager.Instance.playerUIHudManager.SetMaxStamValue(totalStam);
    }

    protected override void Update() {
        base.Update();
    }
}
