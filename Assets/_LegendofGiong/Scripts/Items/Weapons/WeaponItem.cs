using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item {
    public GameObject weaponModel;

    public float damage;

    public float light_Attack_01_Modifier = 1.1f;

    public WeaponItemAction lmb_Action; // left mouse button
}
