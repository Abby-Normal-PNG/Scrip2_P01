using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AbilityCooldown : MonoBehaviour
{
    public event Action AbilityActivated = delegate { };

    public Ability _ability;
    public GameObject _weaponHolder;
    
    public string _abilityButtonAxisName = "Fire1";
    public Image _darkMask;
    public Text _coolDownText;

    private Image _buttonImage;
    private float _cooldownDuration;
    [HideInInspector] public float _nextReadyTime;
    [HideInInspector] public float _cooldownTimeLeft;

    private void Start()
    {
        Initialize(_ability, _weaponHolder);
        SetCooldown();
    }

    public void Initialize(Ability selectedAbility, GameObject weaponHolder)
    {
        _ability = selectedAbility;
        _buttonImage = GetComponent<Image>();
        _buttonImage.sprite = _ability._abilitySprite;
        _darkMask.sprite = _ability._abilitySprite;
        _cooldownDuration = _ability._abilityBaseCooldown;
        _ability.Initialize(weaponHolder);
    }

    private void Update()
    {
        bool _coolDownComplete = (Time.time > _nextReadyTime);
        if (_coolDownComplete)
        {
            AbilityReady();
            if (Input.GetButtonDown(_abilityButtonAxisName))
            {
                ButtonTriggered();
            }
        }
        else
        {
            Cooldown();
        }
    }

    private void AbilityReady()
    {
        _coolDownText.enabled = false;
        _darkMask.enabled = false;
    }

    private void Cooldown()
    {
        _cooldownTimeLeft -= Time.deltaTime;
        float roundedCD = Mathf.Round(_cooldownTimeLeft);
        _darkMask.fillAmount = (_cooldownTimeLeft / _cooldownDuration);
        _coolDownText.text = roundedCD.ToString();
    }

    private void ButtonTriggered()
    {
        SetCooldown();

        AudioHelper.PlayClip2D(_ability._abilityClip, 1f);
        _ability.TriggerAbility();
        AbilityActivated?.Invoke();
    }

    public void SetCooldown()
    {
        _nextReadyTime = _cooldownDuration + Time.time;
        _cooldownTimeLeft = _cooldownDuration;
        _darkMask.enabled = true;
        _coolDownText.enabled = true;
    }
}
