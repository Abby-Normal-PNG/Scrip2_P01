using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    [SerializeField] Ability _ability;
    [SerializeField] GameObject _weaponHolder;
    
    public string _abilityButtonAxisName = "Fire1";
    public Image _darkMask;
    public Text _coolDownText;

    private Image _buttonImage;
    private AudioSource _abilityAudio;
    private float _cooldownDuration;
    private float _nextReadyTime;
    private float _cooldownTimeLeft;

    private void Start()
    {
        Initialize(_ability, _weaponHolder);
    }

    public void Initialize(Ability _selectedAbility, GameObject _weaponHolder)
    {
        _ability = _selectedAbility;
        _buttonImage = GetComponent<Image>();
        _abilityAudio = GetComponent<AudioSource>();
        _buttonImage.sprite = _ability._abilitySprite;
        _darkMask.sprite = _ability._abilitySprite;
        _cooldownDuration = _ability._abilityBaseCooldown;
        _ability.Initialize(_weaponHolder);
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
        float _roundedCD = Mathf.Round(_cooldownTimeLeft);
        _darkMask.fillAmount = (_cooldownTimeLeft / _cooldownDuration);
        _coolDownText.text = _roundedCD.ToString();
    }

    private void ButtonTriggered()
    {
        _nextReadyTime = _cooldownDuration + Time.time;
        _cooldownTimeLeft = _cooldownDuration;
        _darkMask.enabled = true;
        _coolDownText.enabled = true;

        _abilityAudio.clip = _ability._abilityClip;
        _abilityAudio.Play();
        _ability.TriggerAbility();
    }
}
