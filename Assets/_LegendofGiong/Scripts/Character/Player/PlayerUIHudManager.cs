using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] private StatusBar healthBar;
    [SerializeField] private StatusBar stamBar;

    public void SetNewHealthValue(float oldValue, float newValue) {
        healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(float maxHealth) {
        healthBar.SetMaxStat(maxHealth);
    }

    public void SetNewStamValue(float oldValue, float newValue) {
        stamBar.SetStat(newValue);
    }

    public void SetMaxStamValue(float maxStam) {
        stamBar.SetMaxStat(maxStam);
    }
}
