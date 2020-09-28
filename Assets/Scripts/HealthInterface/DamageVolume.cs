using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageVolume : MonoBehaviour
{
    [SerializeField] int _damageAmount = 1;
    [SerializeField] float _knockbackForce = 10;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable<int> damageable = other.GetComponent<IDamageable<int>>();
        if(damageable != null)
        {
            damageable.Damage(_damageAmount);
            ThirdPersonMovement playerMove = other.GetComponent<ThirdPersonMovement>();
            if(playerMove != null)
            {
                playerMove.Knockback(gameObject.transform.position, _knockbackForce);
            }
        }
    }
}
