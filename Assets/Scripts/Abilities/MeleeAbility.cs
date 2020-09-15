using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MeleeAbility")]

public class MeleeAbility : Ability
{
    [Header("Stats")]
    [SerializeField] int _meleeDamage = 1;
    [SerializeField] float _meleeRange = 50f;
    [SerializeField] float _hitForce = 100f;
    [Header("Feedback")]
    [SerializeField] GameObject _successEffects;
    [SerializeField] AudioClip _successClip;
    [SerializeField] float _hitEffectTimeSecs = 0.5f;

    private MeleeTriggerable _melee;
    private ParticleSystem _particle;
    private AudioSource _audio;

    public override void Initialize(GameObject _obj)
    {
        GameObject _initEffects = Instantiate(_successEffects);
        _particle = _initEffects.GetComponentInChildren<ParticleSystem>();
        _audio = _initEffects.GetComponentInChildren<AudioSource>();
        _melee = _obj.GetComponent<MeleeTriggerable>();

        _melee._meleeDamage = _meleeDamage;
        _melee._meleeRange = _meleeRange;
        _melee._hitForce = _hitForce;
        _melee._meleeOrigin = _obj.transform;

        _melee._particle = _particle;
        _melee._audio = _audio;
        _melee._successClip = _successClip;
        _melee._hitEffectTime = new WaitForSeconds(_hitEffectTimeSecs);

        _melee.Initialize();
    }

    public override void TriggerAbility()
    {
        _melee.MeleeAttack();
    }
}
