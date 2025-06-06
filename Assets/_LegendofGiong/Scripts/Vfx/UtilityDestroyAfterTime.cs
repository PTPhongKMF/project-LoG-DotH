using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityDestroyAfterTime : MonoBehaviour {
    [SerializeField] private float timeUntilDestroyed = 5f;

    private void Awake() {
        Destroy(gameObject, timeUntilDestroyed);
    }
}
