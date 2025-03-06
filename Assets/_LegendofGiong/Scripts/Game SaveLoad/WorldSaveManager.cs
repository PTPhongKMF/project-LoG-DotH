using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveManager : MonoBehaviour {
    private static WorldSaveManager instance;
    public static WorldSaveManager Instance {
        get => instance;
        private set => instance = value;
    }

    [SerializeField] private string worldSceneName;

    private PlayerStatsManager playerStatsManager;
    private SaveLoadFileManager saveLoadFileManager;

    public string gamePath;

    [SerializeField] private bool isSavingGame;
    [SerializeField] private bool isLoadingGame;

    public SaveSlot currentCharSaveSlot;
    public CharacterSaveData currentCharData;
    private string saveFileName;

    public CharacterSaveData saveSlot01; public CharacterSaveData saveSlot02; public CharacterSaveData saveSlot03; public CharacterSaveData saveSlot04;
    public CharacterSaveData saveSlot05; public CharacterSaveData saveSlot06; public CharacterSaveData saveSlot07; public CharacterSaveData saveSlot08;
    public CharacterSaveData saveSlot09; public CharacterSaveData saveSlot10; public CharacterSaveData saveSlot11; public CharacterSaveData saveSlot12;
    public CharacterSaveData saveSlot13; public CharacterSaveData saveSlot14; public CharacterSaveData saveSlot15; public CharacterSaveData saveSlot16;
    public CharacterSaveData saveSlot17; public CharacterSaveData saveSlot18; public CharacterSaveData saveSlot19; public CharacterSaveData saveSlot20;

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        if (playerStatsManager == null) {
            playerStatsManager = GameObject.FindWithTag("Player").GetComponent<PlayerStatsManager>();
        }

        SceneManager.activeSceneChanged += OnSceneChange;

        if (!Application.isEditor) {
            gamePath = Directory.GetParent(gamePath).FullName;
        } else {
            gamePath = Path.Combine(Application.dataPath, "_Test SaveLoad");
        }
    }

    private void Start() {

    }

    private void Update() {
        if (isSavingGame) {
            isSavingGame = false;
            SaveGame();
        }
        if (isLoadingGame) {
            isLoadingGame = false;
            LoadGame();
        }
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    public string DecideCharacterSaveFileName(SaveSlot slot) {
        saveFileName = slot.ToString();

        return saveFileName;
    }

    public void NewGame() {
        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");

        foreach (SaveSlot slot in Enum.GetValues(typeof(SaveSlot))) {
            saveLoadFileManager.saveFileName = DecideCharacterSaveFileName(slot);

            if (!saveLoadFileManager.IsSaveFileExists()) {
                currentCharSaveSlot = slot;
                currentCharData = new CharacterSaveData();
                StartCoroutine(LoadWorldScene("IntroCutscene"));
                return;
            }
        }

        MenuScreenManager.Instance.ShowAlertOutOfSaveSlots(true);
    }

    public void LoadGame() {
        DecideCharacterSaveFileName(currentCharSaveSlot);

        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = saveFileName;
        currentCharData = saveLoadFileManager.LoadSaveFile();

        StartCoroutine(LoadWorldScene(currentCharData.worldSceneName));
    }

    public void SaveGame() {
        if (!SceneData.Instance.isSafeToSave) return;

        DecideCharacterSaveFileName(currentCharSaveSlot);

        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = saveFileName;

        WriteGameDataToCurrentCharData();

        saveLoadFileManager.CreateSaveFile(currentCharData);
    }

    public void DeleteGame(SaveSlot slot) {
        saveLoadFileManager = new SaveLoadFileManager();
        saveLoadFileManager.saveFileDirPath = Path.Combine(gamePath, "Data", "Saves");
        saveLoadFileManager.saveFileName = slot.ToString();

        saveLoadFileManager.DeleteSaveFile();
    }

    public void WriteGameDataToCurrentCharData() {
        currentCharData.charName = playerStatsManager.charName;
        currentCharData.secondsPlayed = 1;
        currentCharData.worldSceneName = worldSceneName;
        currentCharData.locationName = SceneData.Instance.locationName;
        currentCharData.xWorldPosition = playerStatsManager.transform.position.x;
        currentCharData.yWorldPosition = playerStatsManager.transform.position.y;
        currentCharData.zWorldPosition = playerStatsManager.transform.position.z;

        currentCharData.levelPoint = playerStatsManager.GetLevelPoint();
        currentCharData.attackPoint = playerStatsManager.AttackPoint;
        currentCharData.healthPoint = playerStatsManager.HealthPoint;
        currentCharData.stamPoint = playerStatsManager.StamPoint;

        // Save weapon equipment data
        PlayerInventoryManager inventory = playerStatsManager.GetComponent<PlayerInventoryManager>();
        PlayerMovementController movement = playerStatsManager.GetComponent<PlayerMovementController>();
        
        currentCharData.currentLeftHandWeaponId = inventory.currentLeftHandWeapon != null ? inventory.currentLeftHandWeapon.itemId : WorldItemDatabase.Instance.unarmedWeapon.itemId;
        currentCharData.currentRightHandWeaponId = inventory.currentRightHandWeapon != null ? inventory.currentRightHandWeapon.itemId : WorldItemDatabase.Instance.unarmedWeapon.itemId;
        
        for (int i = 0; i < 3; i++) {
            currentCharData.weaponsInLeftHandSlots[i] = inventory.weaponsInLeftHandSlots[i].itemId;
            currentCharData.weaponsInRightHandSlots[i] = inventory.weaponsInRightHandSlots[i].itemId;
        }
        
        currentCharData.leftHandWeaponIndex = inventory.leftHandWeaponIndex;
        currentCharData.rightHandWeaponIndex = inventory.rightHandWeaponIndex;
        currentCharData.isArmed = movement.isArmed;
        currentCharData.revertIsArmed = movement.revertIsArmed;

        currentCharData.allUtilitySlotsCanvasGroup = PlayerUIManager.Instance.playerUIHudManager.allUtilitySlotsCanvasGroup.alpha;

        currentCharData.hasHealingAbility = playerStatsManager.playerEquipmentManager.hasHealingAbility;
        currentCharData.hasRageAbility = playerStatsManager.playerEquipmentManager.hasRageAbility;

        currentCharData.hasStartFirstFightAndWeapon = SceneData.Instance.hasStartFirstFightAndWeapon;
        currentCharData.hasFirstFightAndWeapon = SceneData.Instance.hasFirstFightAndWeapon;
    }

    public void ReadGameDataFromCurrentCharData() {
        playerStatsManager.charName = currentCharData.charName;
        Vector3 currentPosition = new Vector3(currentCharData.xWorldPosition, currentCharData.yWorldPosition, currentCharData.zWorldPosition);
        playerStatsManager.transform.position = currentPosition;

        playerStatsManager.AddLevelPoint(currentCharData.levelPoint);
        playerStatsManager.AttackPoint = currentCharData.attackPoint;
        playerStatsManager.HealthPoint = currentCharData.healthPoint;
        playerStatsManager.StamPoint = currentCharData.stamPoint;

        // Load weapon equipment data
        PlayerInventoryManager inventory = playerStatsManager.GetComponent<PlayerInventoryManager>();
        PlayerMovementController movement = playerStatsManager.GetComponent<PlayerMovementController>();
        PlayerEquipmentManager equipment = playerStatsManager.GetComponent<PlayerEquipmentManager>();

        // Load weapons in slots
        for (int i = 0; i < 3; i++) {
            inventory.weaponsInLeftHandSlots[i] = WorldItemDatabase.Instance.GetWeaponById(currentCharData.weaponsInLeftHandSlots[i]);
            inventory.weaponsInRightHandSlots[i] = WorldItemDatabase.Instance.GetWeaponById(currentCharData.weaponsInRightHandSlots[i]);
        }

        // Set current weapons and indexes
        inventory.currentLeftHandWeapon = WorldItemDatabase.Instance.GetWeaponById(currentCharData.currentLeftHandWeaponId);
        inventory.currentRightHandWeapon = WorldItemDatabase.Instance.GetWeaponById(currentCharData.currentRightHandWeaponId);
        inventory.leftHandWeaponIndex = currentCharData.leftHandWeaponIndex;
        inventory.rightHandWeaponIndex = currentCharData.rightHandWeaponIndex;

        // Set armed state
        movement.isArmed = currentCharData.isArmed;
        movement.revertIsArmed = currentCharData.revertIsArmed;

        // Load weapon models
        equipment.LoadWeaponOnBothHands();

        PlayerUIManager.Instance.playerUIHudManager.allUtilitySlotsCanvasGroup.alpha = currentCharData.allUtilitySlotsCanvasGroup;

        playerStatsManager.playerEquipmentManager.hasHealingAbility = currentCharData.hasHealingAbility;
        playerStatsManager.playerEquipmentManager.hasRageAbility = currentCharData.hasRageAbility;

        SceneData.Instance.hasStartFirstFightAndWeapon = currentCharData.hasStartFirstFightAndWeapon;
        SceneData.Instance.hasFirstFightAndWeapon = currentCharData.hasFirstFightAndWeapon;
    }

    public IEnumerator LoadWorldScene(string sceneName) {
        PlayerUIManager.Instance.playerUILoadingScreen.ActivateLoadingScreen();

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        ReadGameDataFromCurrentCharData();

        yield return null;
    }

    private void OnSceneChange(Scene previousScene, Scene currentScene) {
        worldSceneName = SceneManager.GetActiveScene().name;
    }
}
