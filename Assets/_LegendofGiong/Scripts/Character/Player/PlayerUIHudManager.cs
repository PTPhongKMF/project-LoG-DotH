using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] private StatusBar stamBar;

    public void SetNewStamValue(float oldValue, float newValue) {
        stamBar.SetStat(newValue);
    }

    public void SetMaxStamValue(float maxStam) {
        stamBar.SetMaxStat(maxStam);
    }
}
