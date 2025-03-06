using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item {
    public GameObject weaponModel;
    public WeaponType weaponType;

    public float damage;

    public float light_Attack_01_Modifier = 1f;
    public float light_Attack_02_Modifier = 1.5f;
    public float light_Attack_03_Modifier = 2f;

    public float special_Attack_01_Modifier = 5f;

    public WeaponItemAction lmb_Action; // left mouse button
}
