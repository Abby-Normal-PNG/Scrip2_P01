using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCollectible : CollectibleBase
{
    [SerializeField] int _healAmount = 2;
    protected override void Collect(ThirdPersonMovement player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        health.Heal(_healAmount);
    }
}
