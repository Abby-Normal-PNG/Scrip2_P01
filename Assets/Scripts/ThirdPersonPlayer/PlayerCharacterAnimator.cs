using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ThirdPersonInput _thirdPersonInput = null;
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;
    [SerializeField] AbilityCooldown _mainAbility = null;
    [SerializeField] AbilityCooldown _secondaryAbility = null;
    [SerializeField] PlayerHealth _health = null;
    [Header("Feedback")] 
    [SerializeField] ParticleSystem _landParticle = null;
    [SerializeField] AudioClip _hurtClip = null;
    [SerializeField] AudioClip _jumpClip = null;
    [SerializeField] AudioSource _stepAudio = null;

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
        _mainAbility.AbilityActivated += OnAbilityActivated;
        _secondaryAbility.AbilityActivated += OnAbilityActivated;
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
        _mainAbility.AbilityActivated -= OnAbilityActivated;
        _secondaryAbility.AbilityActivated -= OnAbilityActivated;
        _health.TookDamage -= OnTookDamage;
        _health.Died -= OnDied;
    }

    private void OnIdle()
    {
        if (_isDamagedOrDying == false)
        {
            _animator.CrossFadeInFixedTime(IdleState, .2f);
        }
        _stepAudio.Stop();
    }

    private void OnStartRunning()
    {
        if (_isDamagedOrDying == false)
        {
            _animator.CrossFadeInFixedTime(RunState, .2f);
            _stepAudio.Play();
        }
    }

    private void OnStartSprinting()
    {
        if (_isDamagedOrDying == false)
        {
            _landParticle.Play();
            _animator.CrossFadeInFixedTime(SprintState, .2f);
            _stepAudio.Play();
        }
    }

    private void OnJumped()
    {
        if (_isDamagedOrDying == false)
        {
            CancelCoroutines();
            _animCoroutine = StartCoroutine(JumpCoroutine(.5f));
        }
        _stepAudio.Stop();
    }

    private void OnFell()
    {
        if (_isDamagedOrDying == false)
        {
            _animator.CrossFadeInFixedTime(FallState, .2f);
        }
        _stepAudio.Stop();
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
        if (_isDamagedOrDying == false && _thirdPersonMovement._superJumped == false)
        {
            CancelCoroutines();
            _animCoroutine = StartCoroutine(AbilityCoroutine(0.6f));
        }
        if(_thirdPersonMovement._superJumped == true)
        {
            _thirdPersonMovement._superJumped = false;
            _stepAudio.Stop();
        }
    }

    private void OnTookDamage()
    {
        if(_isDamagedOrDying == false)
        {
            CancelCoroutines();
            _animCoroutine = StartCoroutine(DamageCoroutine(.8f));
            _stepAudio.Stop();
        }
    }

    private void OnDied()
    {
        CancelCoroutines();
        _isDamagedOrDying = true;
        _animator.CrossFadeInFixedTime(DeadState, .2f);
        AudioHelper.PlayClip2D(_hurtClip, 1f);
        _stepAudio.Stop();
    }

    private void CancelCoroutines()
    {
        if (_animCoroutine != null)
        {
            StopCoroutine(_animCoroutine);
        }
    }

    IEnumerator JumpCoroutine(float jumpingTimeInSeconds)
    {
        _animator.CrossFadeInFixedTime(LandState, .1f);
        AudioHelper.PlayClip2D(_jumpClip, 1f);
        _landParticle.Play();
        yield return new WaitForSeconds(jumpingTimeInSeconds);
        OnFell();
        _animCoroutine = null;
    }

    IEnumerator LandCoroutine(float landingTimeInSeconds)
    {
        _animator.CrossFadeInFixedTime(LandState, .1f);
        _landParticle.Play();
        yield return new WaitForSeconds(landingTimeInSeconds);
        _thirdPersonInput.RecheckRunSprintIdle();
        _animCoroutine = null;
    }

    IEnumerator AbilityCoroutine(float abilityTimeInSeconds)
    {
        _animator.CrossFadeInFixedTime(AbilityState, .1f);
        yield return new WaitForSeconds(abilityTimeInSeconds);
        _thirdPersonInput.RecheckRunSprintIdle();
        _animCoroutine = null;
    }

    IEnumerator DamageCoroutine(float damagetimeInSeconds)
    {
        _isDamagedOrDying = true;
        _animator.CrossFadeInFixedTime(DamageState, .1f);
        AudioHelper.PlayClip2D(_hurtClip, 1f);
        yield return new WaitForSeconds(damagetimeInSeconds);
        _isDamagedOrDying = false;
        _thirdPersonInput.RecheckRunSprintIdle();
        _animCoroutine = null;
        _isDamagedOrDying = false;
    }
}
