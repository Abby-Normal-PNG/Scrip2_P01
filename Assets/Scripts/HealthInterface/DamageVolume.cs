using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageVolume : MonoBehaviour
{
    [SerializeField] int _damageAmount = 1;
    private void OnTriggerEnter(Collider _other)
    {
        IDamageable<int> _damageable = _other.GetComponent<IDamageable<int>>();
        if(_damageable != null)
        {
            _damageable.Damage(_damageAmount);
        }
    }
}
