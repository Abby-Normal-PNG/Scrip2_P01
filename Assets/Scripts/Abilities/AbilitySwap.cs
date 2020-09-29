using UnityEngine;

public class AbilitySwap : MonoBehaviour
{
    [SerializeField] AbilityCooldown _mainAbilityCD;
    [SerializeField] AbilityCooldown _subAbilityCD;

    public void SwapMainAbility(Ability ability, GameObject weaponHolder)
    {
        _mainAbilityCD.Initialize(ability, weaponHolder);
    }

    public void SwapSubAbility(Ability ability, GameObject weaponHolder)
    {
        _subAbilityCD.Initialize(ability, weaponHolder);
    }

    public void SwitchAbilities()
    {
        //Store abilities and holders temporarily so they aren't overwritten
        Ability ability1 = _mainAbilityCD._ability;
        GameObject holder1 = _mainAbilityCD._weaponHolder;
        Ability ability2 = _subAbilityCD._ability;
        GameObject holder2 = _subAbilityCD._weaponHolder;
        //Initialize main with 2nd, then 2nd with main
        SwapMainAbility(ability2, holder2);
        _mainAbilityCD.SetCooldown();
        SwapSubAbility(ability1, holder1);
        _subAbilityCD.SetCooldown();
    }
}
