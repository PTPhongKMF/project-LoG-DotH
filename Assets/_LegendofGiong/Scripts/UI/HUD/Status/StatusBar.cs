using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusBar : MonoBehaviour {
    private Slider slider;
    [SerializeField] private TextMeshProUGUI currentValueText; 
    [SerializeField] private TextMeshProUGUI maxValueText;

    protected virtual void Awake() {
        slider = GetComponent<Slider>();
    }

    public virtual void SetStat(float newValue, float maxValue) {
        newValue = Mathf.Clamp(Mathf.RoundToInt(newValue), 0, maxValue);
        currentValueText.text = newValue.ToString();
        slider.value = newValue;
    }

    public virtual void SetMaxStat(float maxValue) {
        currentValueText.text = maxValue.ToString();
        maxValueText.text = maxValue.ToString();
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }
}
