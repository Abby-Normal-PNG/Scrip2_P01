using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/JumpAbility")]
public class JumpAbility : Ability
{
    [Header("Stats")]
    [SerializeField] float _jumpSpeed = 60;
    [Header("Feedback")]
    [SerializeField] GameObject _jumpEffects;

    private ThirdPersonMovement _movement;
    public override void Initialize(GameObject obj)
    {
        _movement = obj.GetComponentInParent<ThirdPersonMovement>();
    }

    public override void TriggerAbility()
    {
        _movement.PrepareSuperJump(_jumpSpeed);
        Instantiate(_jumpEffects, _movement.gameObject.transform);
    }
}
