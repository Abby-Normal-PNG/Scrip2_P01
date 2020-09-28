using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour, IDamageable<int>, IKillable, IHealable<int>
{
    public event Action TookDamage = delegate { };
    public event Action Healed = delegate { };
    public event Action Died = delegate { };

    [SerializeField] int _currentHealth = 10;
    [SerializeField] int _maxHealth = 10;

    public void Damage(int damageTaken)
    {
        _currentHealth -= damageTaken;
        CheckDeath();
    }

    void CheckDeath()
    {
        if (_currentHealth <= 0)
        {
            Kill();
        }
        else
        {
            TookDamage?.Invoke();
        }
    }

    public void Heal(int damageHealed)
    {
        _currentHealth += damageHealed;
        CapHealth();
        Healed?.Invoke();
    }

    void CapHealth()
    {
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    public void Kill()
    {
        Died?.Invoke();
    }
}
