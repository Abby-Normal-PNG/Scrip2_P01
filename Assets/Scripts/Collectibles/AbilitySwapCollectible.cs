using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySwapCollectible : CollectibleBase
{
    [SerializeField] Ability _abilityToSwapIn;
    private GameObject _weaponHolder;
    protected override void Collect(ThirdPersonMovement player)
    {
        _weaponHolder = player.GetComponentInChildren<AbilityHolder>().gameObject;
        AbilitySwap swap = player.GetComponent<AbilitySwap>();
        swap.SwapAbility(_abilityToSwapIn, _weaponHolder);
    }
}
