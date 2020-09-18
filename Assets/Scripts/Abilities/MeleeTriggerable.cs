using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTriggerable : MonoBehaviour
{
    [HideInInspector] public int _meleeDamage = 1; 
    [HideInInspector] public float _meleeRange = 50f;
    [HideInInspector] public float _hitForce = 100f;
    [HideInInspector] public Transform _meleeOrigin;

    [HideInInspector] public ParticleSystem _particle;
    [HideInInspector] public AudioSource _audio;
    [HideInInspector] public AudioClip _successClip;
    [HideInInspector] public WaitForSeconds _hitEffectTime;

    private Coroutine _coroutine;

    public void Initialize()
    {
        _audio.clip = _successClip;
    }

    public void MeleeAttack()
    {
        Vector3 _castOrigin = _meleeOrigin.position;
        Debug.DrawRay(_castOrigin, _meleeOrigin.transform.forward * _meleeRange, Color.green);
        RaycastHit _hit;
        if (Physics.Raycast(_castOrigin, _meleeOrigin.transform.forward, out _hit, _meleeRange))
        {
            _coroutine = StartCoroutine(MeleeHitEffect(_hit));
            IDamageable<int> _hitHealth = _hit.collider.gameObject.GetComponent<IDamageable<int>>();
            if (_hitHealth != null)
            {
                _hitHealth.Damage(_meleeDamage);
            }
            if (_hit.rigidbody != null)
            {
                _hit.rigidbody.AddForce(-_hit.normal * _hitForce);
            }
        }
    }

    private IEnumerator MeleeHitEffect(RaycastHit _hit)
    {
        _audio.Play();
        _particle.gameObject.transform.position = _hit.point;
        _particle.Play();
        yield return _hitEffectTime;
        _audio.Stop();
        _particle.Stop();
    }
}
