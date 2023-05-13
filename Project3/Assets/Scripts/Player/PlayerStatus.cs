using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour{
    [Header("Player status")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;

    [Header("Caching attribute")]
    [SerializeField] HealthBar healthBar;

    private void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public float CurrentHealth() {
        return currentHealth;
    }
    public void ChangeHealthBy(float healthChange) {
        currentHealth += healthChange;
        CheckHealthConstraint();
        healthBar.SetHealthBar(currentHealth);
    }
    public void SetHealthTo(float healthValue) {
        currentHealth = healthValue;
        CheckHealthConstraint();
        healthBar.SetHealthBar(currentHealth);
    }
    public bool IsCurrentHealthDepleted() {
        return currentHealth == 0;
    }
    private void CheckHealthConstraint() {
        if (currentHealth < 0) {
            currentHealth = 0;
        } else if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }
    
}
