using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageVolume : MonoBehaviour
{
    [SerializeField] int _damageAmount = 1;
    [SerializeField] float _knockbackForce = 10;

    private void OnTriggerEnter(Collider _other)
    {
        IDamageable<int> _damageable = _other.GetComponent<IDamageable<int>>();
        if(_damageable != null)
        {
            _damageable.Damage(_damageAmount);
            ThirdPersonMovement _playerMove = _other.GetComponent<ThirdPersonMovement>();
            if(_playerMove != null)
            {
                _playerMove.Knockback(gameObject.transform.position, _knockbackForce);
            }
        }
    }
}
