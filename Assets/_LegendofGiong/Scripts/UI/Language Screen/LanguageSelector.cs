using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour {
    private static LanguageSelector instance;
    public static LanguageSelector Instance {
        get => instance;
        private set => instance = value;
    }

    private void Awake() {
        // there can only be one of this instance script at one time, if another exist, destroy it
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        StartCoroutine(LoadStoredLanguage());
    }

    private IEnumerator LoadStoredLanguage() {
        yield return LocalizationSettings.InitializationOperation;

        string localeCode = PlayerPrefs.GetString("language", LocalizationSettings.SelectedLocale.Identifier.Code);
        ChangeLanguage(localeCode);
    }

    public void ChangeLanguage(string localeCode) {
        StartCoroutine(SetLanguage(localeCode));
    }

    private IEnumerator SetLanguage(string localeCode) {
        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == localeCode);
        PlayerPrefs.SetString("language", localeCode);
        PlayerPrefs.Save();
    }
}
