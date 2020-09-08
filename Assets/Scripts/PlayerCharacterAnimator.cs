using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonInput _thirdPersonInput = null;
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;
    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jump";
    const string FallState = "Falling";
    const string LandState = "Land";
    const string SprintState = "Sprint";

    Animator _animator = null;
    Coroutine _landCoroutine = null;

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
    }

    private void OnDisable()
    {
        _thirdPersonInput.Idle -= OnIdle;
        _thirdPersonInput.StartRunning -= OnStartRunning;
        _thirdPersonInput.StartSprinting -= OnStartSprinting;
        _thirdPersonMovement.Jumped -= OnJumped;
        _thirdPersonMovement.Fell -= OnFell;
        _thirdPersonMovement.Landed -= OnLanded;
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
        StopCoroutine(_landCoroutine);
        _animator.CrossFadeInFixedTime(JumpState, .2f);
    }

    private void OnFell()
    {
        StopCoroutine(_landCoroutine);
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }

    private void OnLanded()
    {
        _landCoroutine = StartCoroutine(LandCoroutine(.1f));
    }

    IEnumerator LandCoroutine(float _landingTimeInSeconds)
    {
        _animator.CrossFadeInFixedTime(LandState, _landingTimeInSeconds);
        yield return new WaitForSeconds(_landingTimeInSeconds);
        _thirdPersonInput.RecheckRunSprintIdle();
    }

}
