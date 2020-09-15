using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTriggerable : MonoBehaviour
{
    [HideInInspector] public int _meleeDamage = 1; 
    [HideInInspector] public float _meleeRange = 50f;
    [HideInInspector] public float _hitForce = 100f;
    [HideInInspector] public Transform _kickOrigin;

    public void Initialize()
    {
        
    }

    public void MeleeAttack()
    {
        Vector3 rayOrigin = _kickOrigin.position;
        Debug.DrawRay(rayOrigin, _kickOrigin.transform.forward * _meleeRange, Color.green);
        RaycastHit _hit;
        //StartCoroutine(MeleeEffect());

        //Check if our raycast has hit anything
        if (Physics.Raycast(rayOrigin, _kickOrigin.transform.forward, out _hit, _meleeRange))
        {

            Health _hitHealth = _hit.collider.gameObject.GetComponent<Health>();
            if (_hitHealth != null)
            {
                _hitHealth.TakeDamage(_meleeDamage);
            }
            if (_hit.rigidbody != null)
            {
                _hit.rigidbody.AddForce(-_hit.normal * _hitForce);
            }
        }
    }
}
