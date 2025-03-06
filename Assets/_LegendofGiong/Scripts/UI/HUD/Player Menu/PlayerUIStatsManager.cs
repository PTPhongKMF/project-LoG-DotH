using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class PlayerUIStatsManager : MonoBehaviour {
    private PlayerMovementController player;

    public GameObject statsTab;

    [SerializeField] private TextMeshProUGUI currentTotalLevelText;
    [SerializeField] private TextMeshProUGUI remainLevelPoint;
    [SerializeField] private Image atkIcon;
    [SerializeField] private TextMeshProUGUI atkStats;
    [SerializeField] private Image hpIcon;
    [SerializeField] private TextMeshProUGUI hpStats;
    [SerializeField] private Image stamIcon;
    [SerializeField] private TextMeshProUGUI stamStats;

    private List<Coroutine> pulsingCoroutines = new List<Coroutine>();
    private float pulseMinScale = 0.95f;
    private float pulseMaxScale = 1.05f;
    private float pulseDuration = 1f;

    private Button atkButton;
    private Button hpButton;
    private Button stamButton;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
        
        // Cache button components and add click listeners
        if (atkIcon != null) {
            atkButton = atkIcon.GetComponent<Button>();
            if (atkButton != null) atkButton.onClick.AddListener(OnAttackPointClicked);
        }
        if (hpIcon != null) {
            hpButton = hpIcon.GetComponent<Button>();
            if (hpButton != null) hpButton.onClick.AddListener(OnHealthPointClicked);
        }
        if (stamIcon != null) {
            stamButton = stamIcon.GetComponent<Button>();
            if (stamButton != null) stamButton.onClick.AddListener(OnStaminaPointClicked);
        }
    }

    private void OnDestroy() {
        // Remove listeners to prevent memory leaks
        if (atkButton != null) atkButton.onClick.RemoveListener(OnAttackPointClicked);
        if (hpButton != null) hpButton.onClick.RemoveListener(OnHealthPointClicked);
        if (stamButton != null) stamButton.onClick.RemoveListener(OnStaminaPointClicked);
    }

    public void OnAttackPointClicked() {
        SpendPointAndUpdateUI("atk");
    }

    public void OnHealthPointClicked() {
        SpendPointAndUpdateUI("hp");
    }

    public void OnStaminaPointClicked() {
        SpendPointAndUpdateUI("st");
    }

    private void SpendPointAndUpdateUI(string statType) {
        if (player == null || player.playerStatsManager == null) return;

        // Spend the point
        player.playerStatsManager.SpentLevelPoint(statType);

        // Update UI
        UpdateStatsUI();

        // Check if we can still spend points
        int remainingPoints = player.playerStatsManager.GetLevelPoint();
        bool canSpendPoints = remainingPoints > 0;

        // Update button states
        UpdateButtonInteractability(canSpendPoints);

        // Update pulsing effects
        StopAllPulsingEffects();
        if (canSpendPoints) {
            StartPulsingEffect(atkIcon);
            StartPulsingEffect(hpIcon);
            StartPulsingEffect(stamIcon);
        }
    }

    private void UpdateStatsUI() {
        if (player == null || player.playerStatsManager == null) return;

        string localizedRemainLevelPoint = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "remain_lvl_point");
        string localizedAtkPoints = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "atk_point");
        string localizedAtkValues = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "total_atk_value");
        string localizedHpPoints = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "hp_point");
        string localizedHpValues = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "total_hp_value");
        string localizedStaPoints = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "stam_point");
        string localizedStaValues = LocalizationSettings.StringDatabase.GetLocalizedString("UIText", "total_stam_value");

        // Update total level and remaining points
        currentTotalLevelText.text = player.playerStatsManager.GetTotalStatsPoint().ToString();
        remainLevelPoint.text = localizedRemainLevelPoint + ": " + player.playerStatsManager.GetLevelPoint().ToString();

        // Update individual stat displays
        atkStats.text = localizedAtkPoints + ": " + player.playerStatsManager.AttackPoint.ToString() + "<br>" 
                      + localizedAtkValues + ": " + player.playerStatsManager.totalAttack;
        hpStats.text = localizedHpPoints + ": " + player.playerStatsManager.HealthPoint.ToString() + "<br>"
                      + localizedHpValues + ": " + player.playerStatsManager.totalHealth;
        stamStats.text = localizedStaPoints + ": " + player.playerStatsManager.StamPoint.ToString() + "<br>"
                      + localizedStaValues + ": " + player.playerStatsManager.totalStam;
    }

    private void OnDisable() {
        StopAllPulsingEffects();
    }

    private void StopAllPulsingEffects() {
        if (pulsingCoroutines == null) return;

        foreach (var coroutine in pulsingCoroutines) {
            if (coroutine != null) {
                StopCoroutine(coroutine);
            }
        }
        pulsingCoroutines.Clear();

        // Reset icon scales safely
        if (atkIcon != null) atkIcon.transform.localScale = Vector3.one;
        if (hpIcon != null) hpIcon.transform.localScale = Vector3.one;
        if (stamIcon != null) stamIcon.transform.localScale = Vector3.one;
    }

    private IEnumerator PulseIcon(Transform iconTransform) {
        if (iconTransform == null) yield break;

        while (true) {
            // Pulse out
            float elapsedTime = 0f;
            Vector3 startScale = Vector3.one * pulseMinScale;
            Vector3 endScale = Vector3.one * pulseMaxScale;

            while (elapsedTime < pulseDuration / 2) {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / (pulseDuration / 2);
                iconTransform.localScale = Vector3.Lerp(startScale, endScale, progress);
                yield return null;
            }

            // Pulse in
            elapsedTime = 0f;
            while (elapsedTime < pulseDuration / 2) {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / (pulseDuration / 2);
                iconTransform.localScale = Vector3.Lerp(endScale, startScale, progress);
                yield return null;
            }
        }
    }

    private void StartPulsingEffect(Image icon) {
        if (icon != null && icon.transform != null) {
            pulsingCoroutines.Add(StartCoroutine(PulseIcon(icon.transform)));
        }
    }

    private void UpdateButtonInteractability(bool canSpendPoints) {
        if (atkButton != null) atkButton.interactable = canSpendPoints;
        if (hpButton != null) hpButton.interactable = canSpendPoints;
        if (stamButton != null) stamButton.interactable = canSpendPoints;
    }

    public void ToggleStatsTab() {
        if (PlayerInputController.Instance == null || PlayerInputController.Instance.IsInputBlocked)
            return;

        if (statsTab.activeSelf) {
            statsTab.SetActive(false);
            StopAllPulsingEffects();
        } else {
            UpdateStatsUI();

            int remainingPoints = player.playerStatsManager.GetLevelPoint();
            bool canSpendPoints = remainingPoints > 0;

            // Update button interactability
            UpdateButtonInteractability(canSpendPoints);

            // Start pulsing effects if points are available
            if (canSpendPoints) {
                StartPulsingEffect(atkIcon);
                StartPulsingEffect(hpIcon);
                StartPulsingEffect(stamIcon);
            }

            statsTab.SetActive(true);
        }
    }
}
