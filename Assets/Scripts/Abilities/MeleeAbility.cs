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

    private MeleeTriggerable _meleeTrigger;

    public override void Initialize(GameObject obj)
    {
        _meleeTrigger = obj.GetComponent<MeleeTriggerable>();

        SetTriggerableStats();
        _meleeTrigger._meleeOrigin = obj.transform;

        _meleeTrigger._hitEffects = _successEffects;
        _meleeTrigger._successClip = _successClip;

        _meleeTrigger.Initialize();
    }

    private void SetTriggerableStats()
    {
        _meleeTrigger._meleeDamage = _meleeDamage;
        _meleeTrigger._meleeRange = _meleeRange;
        _meleeTrigger._hitForce = _hitForce;
    }

    public override void TriggerAbility()
    {
        SetTriggerableStats();
        _meleeTrigger.MeleeAttack();
    }
}
