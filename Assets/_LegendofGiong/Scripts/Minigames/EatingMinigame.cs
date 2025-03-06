using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class EatingMinigame : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private GameObject minigameContainer;
    [SerializeField] private CanvasGroup containerCanvasGroup;
    [SerializeField] private Image overlayImage;
    [SerializeField] private Image[] riceBowlImages;
    [SerializeField] private Image spacebarImage;
    [SerializeField] private TextMeshProUGUI spacebarText;
    [SerializeField] private TextMeshProUGUI clickCounterText;
    [SerializeField] private TextMeshProUGUI targetClicksText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Settings")]
    [SerializeField] private float overlayAlpha = 0.5f;
    [SerializeField] private float defaultRotationSpeed = 45f;
    [SerializeField] private float maxRotationSpeed = 720f;  // Fast rotation when clicking
    [SerializeField] private float rotationDeceleration = 720f;  // How fast it slows down
    private float currentRotationSpeed;
    [SerializeField] private float spacebarPulseMinScale = 0.9f;
    [SerializeField] private float spacebarPulseMaxScale = 1.1f;
    [SerializeField] private float spacebarPulseDuration = 0.1f;

    private int targetClicks;
    private float timeLimit;
    private float currentTime;
    private int currentClicks;
    public bool isActive { get; private set; }
    private Action<bool> onGameComplete;
    private float spacebarPulseTimer;
    private bool isPulsingSpacebar;
    private bool hasInitialized = false;

    // Add these new fields for cached localized text formats
    private string targetClicksFormat = "Target: {0} clicks";
    private string clicksFormat = "Clicks: {0}";
    private string timeFormat = "Time: {0:F1}s";

    private void Awake() {
        if (containerCanvasGroup == null) {
            containerCanvasGroup = GetComponent<CanvasGroup>();
        }

        // Only deactivate on first initialization
        if (!hasInitialized && minigameContainer != null) {
            minigameContainer.SetActive(false);
            hasInitialized = true;
        }

        isActive = false;

        // Load localized strings
        LoadLocalizedStrings();
    }

    private void LoadLocalizedStrings() {
        var settings = LocalizationSettings.StringDatabase;
        targetClicksFormat = settings.GetLocalizedString("UIText", "target_clicks") ?? "Target: {0} clicks";
        clicksFormat = settings.GetLocalizedString("UIText", "current_clicks") ?? "Clicks: {0}";
        timeFormat = settings.GetLocalizedString("UIText", "time_remaining") ?? "Time: {0:F1}s";
    }

    public void StartMinigame(Action<bool> completionCallback) {
        // First activate the main GameObject if it's inactive
        if (!gameObject.activeInHierarchy) {
            gameObject.SetActive(true);
        }
        
        onGameComplete = completionCallback;
        
        // Generate random target and time
        targetClicks = UnityEngine.Random.Range(50, 501);
        timeLimit = CalculateTimeLimit(targetClicks);
        currentTime = timeLimit;
        currentClicks = 0;
        currentRotationSpeed = defaultRotationSpeed;

        // Setup UI
        UpdateUI();
        if (overlayImage != null) {
            Color c = overlayImage.color;
            c.a = overlayAlpha;
            overlayImage.color = c;
        }

        // Disable player movement
        if (PlayerInputController.Instance != null) {
            var player = PlayerInputController.Instance.playerMovementController;
            if (player != null) {
                if (player.playerLocomotionController != null) {
                    player.playerLocomotionController.enabled = false;
                }
                player.enabled = false;
            }
            PlayerInputController.Instance.SetDialogState(true);
        }

        // Activate the container and start game
        if (minigameContainer != null) {
            minigameContainer.SetActive(true);
        } else {
            return; // Don't proceed if container is missing
        }

        isActive = true;
    }

    private void Update() {
        if (!isActive) return;

        // Update timer
        currentTime -= Time.deltaTime;
        UpdateUI();

        // Check for spacebar input
        if (Input.GetKeyDown(KeyCode.Space)) {
            currentClicks++;
            UpdateUI();
            StartSpacebarPulse();
            // Immediate speed boost when clicking!
            currentRotationSpeed = maxRotationSpeed;
        }
        else {
            // No new clicks, slow down
            currentRotationSpeed -= rotationDeceleration * Time.deltaTime;
            if (currentRotationSpeed < defaultRotationSpeed) {
                currentRotationSpeed = defaultRotationSpeed;
            }
        }

        // Rotate all rice bowls
        if (riceBowlImages != null) {
            foreach (var bowlImage in riceBowlImages) {
                if (bowlImage != null) {
                    bowlImage.transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);
                }
            }
        }

        // Handle spacebar pulse animation
        if (isPulsingSpacebar) {
            UpdateSpacebarPulse();
        }

        // Check win/lose conditions
        if (currentClicks >= targetClicks) {
            EndGame(true);
        } else if (currentTime <= 0) {
            EndGame(false);
        }
    }

    private void StartSpacebarPulse() {
        if (spacebarImage == null) return;
        isPulsingSpacebar = true;
        spacebarPulseTimer = 0f;
    }

    private void UpdateSpacebarPulse() {
        if (spacebarImage == null) return;

        spacebarPulseTimer += Time.deltaTime;
        float progress = spacebarPulseTimer / spacebarPulseDuration;

        if (progress >= 1f) {
            spacebarImage.transform.localScale = Vector3.one;
            isPulsingSpacebar = false;
            return;
        }

        float scale;
        if (progress < 0.5f) {
            // Pulse out
            scale = Mathf.Lerp(spacebarPulseMinScale, spacebarPulseMaxScale, progress * 2f);
        } else {
            // Pulse in
            scale = Mathf.Lerp(spacebarPulseMaxScale, spacebarPulseMinScale, (progress - 0.5f) * 2f);
        }
        spacebarImage.transform.localScale = Vector3.one * scale;
    }

    private float CalculateTimeLimit(int clicks) {
        float baseTime = (clicks / 10f) * 2f;
        return baseTime * UnityEngine.Random.Range(0.9f, 1.1f);
    }

    private void UpdateUI() {
        if (targetClicksText != null) {
            targetClicksText.text = string.Format(targetClicksFormat, targetClicks);
        }

        if (clickCounterText != null) {
            clickCounterText.text = string.Format(clicksFormat, currentClicks);
        }

        if (timerText != null) {
            timerText.text = string.Format(timeFormat, Mathf.Round(currentTime));
        }
    }

    private void EndGame(bool won) {
        isActive = false;
        
        // Re-enable player movement
        if (PlayerInputController.Instance != null) {
            var player = PlayerInputController.Instance.playerMovementController;
            if (player != null) {
                player.enabled = true;
                if (player.playerLocomotionController != null) {
                    player.playerLocomotionController.enabled = true;
                }
            }
            PlayerInputController.Instance.SetDialogState(false);
        }

        // Hide UI
        if (minigameContainer != null) {
            minigameContainer.SetActive(false);
        }

        // Trigger callback
        onGameComplete?.Invoke(won);
    }

    public void ForceEndGame() {
        if (isActive) {
            EndGame(false);
        }
    }

    private void OnDisable() {
        if (isActive) {
            ForceEndGame();
        }
    }
} 