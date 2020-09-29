using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(ThirdPersonMovement))]
[RequireComponent(typeof(ThirdPersonInput))]
public class PlayerHealth : MonoBehaviour, IHealable<int>, IDamageable<int>, IKillable
{
    public event Action TookDamage = delegate { };
    public event Action Healed = delegate { };
    public event Action Died = delegate { };

    [Header("Values")]
    [SerializeField] int _currentHealth = 10;
    [SerializeField] int _maxHealth = 10;
    [SerializeField] Slider _healthSlider = null;
    [Header("Damage")]
    [SerializeField] CanvasGroup _damageCG;
    [SerializeField] float _damageFlashTime = 0.5f;
    [SerializeField] float _damageInvulnTime = 0.5f;
    [Header("Heal")]
    [SerializeField] CanvasGroup _healCG;
    [SerializeField] float _healFlashTime = 0.5f;

    private ThirdPersonInput _input;
    private ThirdPersonMovement _movement;
    private Coroutine _coroutine;
    private float _damageInvulnTimeLeft;
    private bool _isDamageable;

    void Start()
    {
        _input = GetComponent<ThirdPersonInput>();
        _movement = GetComponent<ThirdPersonMovement>();
        InitializeHPSlider();
    }

    void InitializeHPSlider()
    {
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.minValue = 0;
        UpdateHPSlider();
    }

    void UpdateHPSlider()
    {
        _healthSlider.value = _currentHealth;
    }

    void Update()
    {
        InvulnTimer();
    }

    private void InvulnTimer()
    {
        if (_isDamageable == false)
        {
            _damageInvulnTimeLeft -= Time.deltaTime;
        }
        if (_damageInvulnTimeLeft <= 0)
        {
            _isDamageable = true;
            _damageInvulnTimeLeft = 0;
        }
    }

    public void Damage(int _damageTaken)
    {
        if (_isDamageable)
        {
            _currentHealth -= _damageTaken;
            _isDamageable = false;
            _damageInvulnTimeLeft = _damageInvulnTime;
            TookDamage?.Invoke();
            DamageEffects();
            CheckDeath();
        }
    }

    private void DamageEffects()
    {
        UpdateHPSlider();
        _coroutine = StartCoroutine(DamageCoroutine(_damageFlashTime));
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
        HealEffects();
        CapHealth();
    }
    private void HealEffects()
    {
        UpdateHPSlider();
        _coroutine = StartCoroutine(HealCoroutine(_healFlashTime));
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
        _input.enabled = false;
        _movement.enabled = false;
        Died?.Invoke();
    }

    IEnumerator DamageCoroutine(float _duration)
    {
        _damageCG.alpha = 1;
        yield return new WaitForSeconds(_duration);
        _damageCG.alpha = 0;
    }
    IEnumerator HealCoroutine(float _duration)
    {
        _healCG.alpha = 1;
        yield return new WaitForSeconds(_duration);
        _healCG.alpha = 0;
    }
}
