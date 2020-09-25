using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonInput _thirdPersonInput = null;
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;
    [SerializeField] AbilityCooldown _ability = null;
    [SerializeField] PlayerHealth _health = null;

    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jump";
    const string FallState = "Falling";
    const string LandState = "Land";
    const string SprintState = "Sprint";
    const string AbilityState = "Kick";
    const string DamageState = "Damaged";
    const string DeadState = "Dying";

    Animator _animator = null;
    Coroutine _animCoroutine = null;

    private bool _isDamagedOrDying = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //Setting Input-based animations from TPInput
        _thirdPersonInput.Idle += OnIdle;
        _thirdPersonInput.StartRunning += OnStartRunning;
        _thirdPersonInput.StartSprinting += OnStartSprinting;
        //Setting Position-based animations from TPMovement
        _thirdPersonMovement.Jumped += OnJumped;
        _thirdPersonMovement.Fell += OnFell;
        _thirdPersonMovement.Landed += OnLanded;
        //Setting ability animation
        _ability.AbilityActivated += OnAbilityActivated;
        //Setting health related animations
        _health.TookDamage += OnTookDamage;
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _thirdPersonInput.Idle -= OnIdle;
        _thirdPersonInput.StartRunning -= OnStartRunning;
        _thirdPersonInput.StartSprinting -= OnStartSprinting;
        _thirdPersonMovement.Jumped -= OnJumped;
        _thirdPersonMovement.Fell -= OnFell;
        _thirdPersonMovement.Landed -= OnLanded;
        _ability.AbilityActivated -= OnAbilityActivated;
        _health.TookDamage -= OnTookDamage;
        _health.Died -= OnDied;
    }

    private void OnIdle()
    {
        if (_isDamagedOrDying == false)
        {
            _animator.CrossFadeInFixedTime(IdleState, .2f);
        }
    }

    private void OnStartRunning()
    {
        if (_isDamagedOrDying == false)
        {
            _animator.CrossFadeInFixedTime(RunState, .2f);
        }
    }

    private void OnStartSprinting()
    {
        if (_isDamagedOrDying == false)
        {
            _animator.CrossFadeInFixedTime(SprintState, .2f);
        }
    }

    private void OnJumped()
    {
        if (_isDamagedOrDying == false)
        {
            CancelCoroutines();
            _animCoroutine = StartCoroutine(JumpCoroutine(.5f));
        }
    }

    private void OnFell()
    {
        if (_isDamagedOrDying == false)
        {
            _animator.CrossFadeInFixedTime(FallState, .2f);
        }
    }

    private void OnLanded()
    {
        if (_isDamagedOrDying == false)
        {
        CancelCoroutines();
        _animCoroutine = StartCoroutine(LandCoroutine(.1f));
        }
    }

    private void OnAbilityActivated()
    {
        if (_isDamagedOrDying == false)
        {
            CancelCoroutines();
            _animCoroutine = StartCoroutine(AbilityCoroutine(0.6f));
        }
    }

    private void OnTookDamage()
    {
        if(_isDamagedOrDying == false)
        {
            CancelCoroutines();
            _animCoroutine = StartCoroutine(DamageCoroutine(1f));
        }
    }

    private void OnDied()
    {
        CancelCoroutines();
        _isDamagedOrDying = true;
        _animator.CrossFadeInFixedTime(DeadState, .2f);
    }

    private void CancelCoroutines()
    {
        if (_animCoroutine != null)
        {
            StopCoroutine(_animCoroutine);
        }
    }

    IEnumerator JumpCoroutine(float _jumpingTimeInSeconds)
    {
        _animator.CrossFadeInFixedTime(LandState, .1f);
        yield return new WaitForSeconds(_jumpingTimeInSeconds);
        OnFell();
        _animCoroutine = null;
    }

    IEnumerator LandCoroutine(float _landingTimeInSeconds)
    {
        _animator.CrossFadeInFixedTime(LandState, .1f);
        yield return new WaitForSeconds(_landingTimeInSeconds);
        _thirdPersonInput.RecheckRunSprintIdle();
        _animCoroutine = null;
    }

    IEnumerator AbilityCoroutine(float _abilityTimeInSeconds)
    {
        _animator.CrossFadeInFixedTime(AbilityState, .1f);
        yield return new WaitForSeconds(_abilityTimeInSeconds);
        _thirdPersonInput.RecheckRunSprintIdle();
        _animCoroutine = null;
    }

    IEnumerator DamageCoroutine(float _damagetimeInSeconds)
    {
        _isDamagedOrDying = true;
        _animator.CrossFadeInFixedTime(DamageState, .1f);
        yield return new WaitForSeconds(_damagetimeInSeconds);
        _thirdPersonInput.RecheckRunSprintIdle();
        _animCoroutine = null;
        _isDamagedOrDying = false;
    }
}
