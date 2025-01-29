using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenManager : MonoBehaviour {
    

    public void StartNewGame() {
        StartCoroutine(WorldManager.Instance.LoadNewGame());
    }
}
