using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonInput _thirdPersonInput = null;
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;
    [SerializeField] AbilityCooldown _ability = null;
    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jump";
    const string FallState = "Falling";
    const string LandState = "Land";
    const string SprintState = "Sprint";
    const string AbilityState = "Jump";

    Animator _animator = null;
    Coroutine _landCoroutine = null;
    Coroutine _abilityCoroutine = null;

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
    }

    private void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }

    private void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
    }

    private void OnStartSprinting()
    {
        _animator.CrossFadeInFixedTime(SprintState, .2f);
    }

    private void OnJumped()
    {
        _animator.CrossFadeInFixedTime(JumpState, .2f);
    }

    private void OnFell()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }

    private void OnLanded()
    {
        CancelCoroutines();
        _landCoroutine = StartCoroutine(LandCoroutine(.1f));
    }

    private void OnAbilityActivated()
    {
        CancelCoroutines();
        _abilityCoroutine = StartCoroutine(AbilityCoroutine(.2f));
    }

    private void CancelCoroutines()
    {
        if(_landCoroutine != null)
        {
            StopCoroutine(_landCoroutine);
        }
        if (_abilityCoroutine != null)
        {
            StopCoroutine(_abilityCoroutine);
        }
    }

    IEnumerator LandCoroutine(float _landingTimeInSeconds)
    {
        _animator.CrossFadeInFixedTime(LandState, _landingTimeInSeconds);
        yield return new WaitForSeconds(_landingTimeInSeconds);
        _thirdPersonInput.RecheckRunSprintIdle();
    }

    IEnumerator AbilityCoroutine(float _abilityTimeInSeconds)
    {
        _animator.CrossFadeInFixedTime(AbilityState, _abilityTimeInSeconds);
        yield return new WaitForSeconds(_abilityTimeInSeconds);
        _thirdPersonInput.RecheckRunSprintIdle();
    }

}
