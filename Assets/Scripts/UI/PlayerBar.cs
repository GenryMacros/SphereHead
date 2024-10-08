using System;
using TMPro;
using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    public GameObject healthBar;
    public TMP_Text gunName;

    public float maxHealth;
    private float _currentHealth;
    
    void Start()
    {
        _currentHealth = maxHealth;
    }
    
    public void Heal(float amount)
    {
        _currentHealth = Math.Min(maxHealth, _currentHealth + amount);
        UpdateHealthBar();
    }
    
    public void Damage(float amount)
    {
        _currentHealth = Math.Max(0.0f, _currentHealth - amount);
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3(_currentHealth * 1.0f / maxHealth * 1.0f,
                                                      healthBar.transform.localScale.y,
                                                      healthBar.transform.localScale.z);
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void ResetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        _currentHealth = maxHealth;
    }
}
