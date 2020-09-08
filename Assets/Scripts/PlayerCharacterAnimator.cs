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

    Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _thirdPersonInput.Idle += OnIdle;
        _thirdPersonInput.StartRunning += OnStartRunning;
        _thirdPersonMovement.Jumped += OnJumped;
        _thirdPersonMovement.Fell += OnFell;
    }

    private void OnDisable()
    {
        _thirdPersonInput.Idle -= OnIdle;
        _thirdPersonInput.StartRunning -= OnStartRunning;
        _thirdPersonMovement.Jumped -= OnJumped;
        _thirdPersonMovement.Fell -= OnFell;
    }

    private void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }

    private void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
    }

    private void OnJumped()
    {
        _animator.CrossFadeInFixedTime(JumpState, .2f);
    }

    private void OnFell()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }

}
