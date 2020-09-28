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

    private MeleeTriggerable _meleeTrigger;
    private ParticleSystem _particle;
    private AudioSource _audio;

    public override void Initialize(GameObject obj)
    {
        GameObject initEffects = Instantiate(_successEffects);
        _particle = initEffects.GetComponentInChildren<ParticleSystem>();
        _audio = initEffects.GetComponentInChildren<AudioSource>();
        _meleeTrigger = obj.GetComponent<MeleeTriggerable>();

        _meleeTrigger._meleeDamage = _meleeDamage;
        _meleeTrigger._meleeRange = _meleeRange;
        _meleeTrigger._hitForce = _hitForce;
        _meleeTrigger._meleeOrigin = obj.transform;

        _meleeTrigger._particle = _particle;
        _meleeTrigger._audio = _audio;
        _meleeTrigger._successClip = _successClip;
        _meleeTrigger._hitEffectTime = new WaitForSeconds(_hitEffectTimeSecs);

        _meleeTrigger.Initialize();
    }

    public override void TriggerAbility()
    {
        _meleeTrigger.MeleeAttack();
    }
}
