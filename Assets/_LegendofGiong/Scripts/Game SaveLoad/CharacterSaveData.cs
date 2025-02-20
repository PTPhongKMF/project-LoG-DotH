using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData {
    public string charName;

    public float secondsPlayed;

    public string worldSceneName;
    public string locationName;
    public float xWorldPosition;
    public float yWorldPosition;
    public float zWorldPosition;

    public int healthPoint;
    public int stamPoint;
}
