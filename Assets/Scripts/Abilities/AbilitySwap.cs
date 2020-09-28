using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySwap : MonoBehaviour
{
    [SerializeField] AbilityCooldown _cooldown;

    public void SwapAbility(Ability ability, GameObject weaponHolder)
    {
        _cooldown.Initialize(ability, weaponHolder);
    }
}
