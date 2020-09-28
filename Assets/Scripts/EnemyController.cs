using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyHealth _health = null;
    [SerializeField] MeshRenderer _bodyArt;
    [SerializeField] Color _damagedColor = Color.grey;
    [SerializeField] Color _healedColor = Color.green;
    [SerializeField] GameObject _deathEffects = null;
    private Color _normalColor;

    private void OnEnable()
    {
        _health.TookDamage += OnTookDamage;
        _health.Healed += OnHealed;
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _health.TookDamage -= OnTookDamage;
        _health.Healed -= OnHealed;
        _health.Died -= OnDied;
    }

    private void Start()
    {
        _normalColor = _bodyArt.material.color;
    }

    void OnTookDamage()
    {
        StartCoroutine(DamageCoroutine(0.5f));
    }

    IEnumerator DamageCoroutine(float _damageTime)
    {
        _bodyArt.material.color = _damagedColor;
        yield return new WaitForSeconds(_damageTime);
        _bodyArt.material.color = _normalColor;
    }

    void OnHealed()
    {
        StartCoroutine(HealCoroutine(0.5f));
    }

    IEnumerator HealCoroutine(float _damageTime)
    {
        _bodyArt.material.color = _healedColor;
        yield return new WaitForSeconds(_damageTime);
        _bodyArt.material.color = _normalColor;
    }

    void OnDied()
    {
        StartCoroutine(DeadCoroutine(0.5f));
    }

    IEnumerator DeadCoroutine(float _damageTime)
    {
        _bodyArt.gameObject.SetActive(false);
        Instantiate(_deathEffects, gameObject.transform.position, gameObject.transform.rotation);
        yield return new WaitForSeconds(_damageTime);
        Destroy(gameObject);
    }
}
