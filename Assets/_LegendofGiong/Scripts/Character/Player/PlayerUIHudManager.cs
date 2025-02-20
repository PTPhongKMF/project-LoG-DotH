using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] private StatusBar healthBar;
    [SerializeField] private StatusBar stamBar;

    public void SetNewHealthValue(float oldValue, float newValue, float maxValue) {
        healthBar.SetStat(newValue, maxValue);
    }

    public void SetMaxHealthValue(float maxHealth) {
        healthBar.SetMaxStat(maxHealth);
    }

    public void SetNewStamValue(float oldValue, float newValue, float maxValue) {
        stamBar.SetStat(newValue, maxValue);
    }

    public void SetMaxStamValue(float maxStam) {
        stamBar.SetMaxStat(maxStam);
    }
}
