using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour{
    private Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
    }
    public void SetHealthBar(float healthValue) {
        slider.value = healthValue;
    }
    public void SetMaxHealth(float maxHealth) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
}
