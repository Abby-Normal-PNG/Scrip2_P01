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
        Vector3 castOrigin = _meleeOrigin.position;
        Debug.DrawRay(castOrigin, _meleeOrigin.transform.forward * _meleeRange, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(castOrigin, _meleeOrigin.transform.forward, out hit, _meleeRange))
        {
            _coroutine = StartCoroutine(MeleeHitEffect(hit));
            IDamageable<int> hitHealth = hit.collider.gameObject.GetComponent<IDamageable<int>>();
            if (hitHealth != null)
            {
                hitHealth.Damage(_meleeDamage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * _hitForce);
            }
        }
    }

    private IEnumerator MeleeHitEffect(RaycastHit hit)
    {
        AudioHelper.PlayClip2D(_successClip, 1f);
        _particle.gameObject.transform.position = hit.point;
        _particle.Play();
        yield return _hitEffectTime;
        _particle.Stop();
    }
}
