using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MeleeAbility")]

public class MeleeAbility : Ability
{
    [SerializeField] int _meleeDamage = 1;
    [SerializeField] float _meleeRange = 50f;
    [SerializeField] float _hitForce = 100f;

    private MeleeTriggerable _melee;
    public override void Initialize(GameObject _obj)
    {
        _melee = _obj.GetComponent<MeleeTriggerable>();
        _melee.Initialize();

        _melee._meleeDamage = _meleeDamage;
        _melee._meleeRange = _meleeRange;
        _melee._hitForce = _hitForce;
        _melee._kickOrigin = _obj.transform;
    }

    public override void TriggerAbility()
    {
        _melee.MeleeAttack();
    }
}
