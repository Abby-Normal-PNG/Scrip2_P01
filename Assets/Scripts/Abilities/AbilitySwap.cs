using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySwap : MonoBehaviour
{
    [SerializeField] AbilityCooldown _mainAbility;
    [SerializeField] AbilityCooldown _secondaryAbility;

    public void SwapMainAbility(Ability ability, GameObject weaponHolder)
    {
        _mainAbility.Initialize(ability, weaponHolder);
    }

    public void SwapSecondaryAbility(Ability ability, GameObject weaponHolder)
    {
        _secondaryAbility.Initialize(ability, weaponHolder);
    }

    public void SwitchAbilities()
    {
        //Store abilities and holders temporarily so they aren't overwritten
        Ability ability1 = _mainAbility._ability;
        GameObject holder1 = _mainAbility._weaponHolder;
        Ability ability2 = _secondaryAbility._ability;
        GameObject holder2 = _secondaryAbility._weaponHolder;
        //Initialize main with 2nd, then 2nd with main
        _mainAbility.Initialize(ability2, holder2);
        _secondaryAbility.Initialize(ability1, holder1);
    }
}
