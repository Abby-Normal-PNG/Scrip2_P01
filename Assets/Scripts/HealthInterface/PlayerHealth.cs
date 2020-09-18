using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerHealth : MonoBehaviour, IHealable<int>, IDamageable<int>, IKillable
{
    public event Action TookDamage = delegate { };
    public event Action Healed = delegate { };
    public event Action Died = delegate { };

    [SerializeField] int _currentHealth = 10;
    [SerializeField] int _maxHealth = 10;

    public void Damage(int _damageTaken)
    {
        _currentHealth -= _damageTaken;
        Debug.Log(gameObject.name + " took " + _damageTaken + " damage");
        CheckDeath();
    }

    void CheckDeath()
    {
        if(_currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Heal(int _damageHealed)
    {
        _currentHealth += _damageHealed;
        CapHealth();
    }

    void CapHealth()
    {
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    public void Kill()
    {
        Scene _currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(_currentScene.name);
    }
}
