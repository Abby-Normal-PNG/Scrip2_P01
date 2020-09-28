using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonInput : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action StartSprinting = delegate { };

    [SerializeField] ThirdPersonMovement _movement = null;
    [SerializeField] AbilitySwap _swap = null;
    [SerializeField] KeyCode _jumpKey1 = KeyCode.Space;
    [SerializeField] KeyCode _jumpKey2 = KeyCode.Return;
    [SerializeField] KeyCode _sprintKey1 = KeyCode.LeftShift;
    [SerializeField] KeyCode _sprintKey2 = KeyCode.RightShift;
    [SerializeField] KeyCode _abilitySwitchKey = KeyCode.Tab;

    bool _isMoving = false;
    bool _isSprinting = false;
    private float _horizontal, _vertical;
    private Vector3 _direction;

    private void Start()
    {
        Idle?.Invoke();
    }

    private void Update()
    {
        ProcessInput();
        _movement.PrepareToMove(_direction);
        if (_direction.magnitude >= 0.1f)
        {
            CheckIfStartedMoving();
        }
        else
        {
            CheckIfStoppedMoving();
        }
    }

    private void ProcessInput()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(_horizontal, 0, _vertical).normalized;

        if (Input.GetKeyDown(_jumpKey1) || Input.GetKeyDown(_jumpKey2)){
            //Debug.Log("Jump Pressed");
            _movement.PrepareJump();
        }

        if (Input.GetKeyDown(_sprintKey1) || Input.GetKeyDown(_sprintKey2))
        {
            OnSprintPress();
        }

        if (Input.GetKeyUp(_sprintKey1) || Input.GetKeyUp(_sprintKey2))
        {
            OnSprintRelease();
        }

        if (Input.GetKeyDown(_abilitySwitchKey))
        {
            SwitchAbilities();
        }
    }

    private void CheckIfStartedMoving()
    {
        if (_isMoving == false && _movement.IsGrounded == true)
        {
            if (_isSprinting)
            {
                StartSprinting?.Invoke();
                //Debug.Log("Started Sprinting");
            }
            else
            {
                StartRunning?.Invoke();
                //Debug.Log("Started Running");
            }
        }
        _isMoving = true;
    }

    private void CheckIfStoppedMoving()
    {
        if (_isMoving == true && _movement.IsGrounded == true)
        {
            Idle?.Invoke();
            //Debug.Log("Stopped Running");
        }
        _isMoving = false;
    }

    public void RecheckRunSprintIdle()
    {
        if (_movement.IsGrounded)
        {
            if (_isMoving)
            {
                if (_isSprinting)
                {
                    StartSprinting?.Invoke();
                    //Debug.Log("Land & Sprint");
                }
                else
                {
                    StartRunning?.Invoke();
                    //Debug.Log("Land & Run");
                }
            }
            else
            {
                Idle?.Invoke();
                //Debug.Log("Land & Idle");
            }
        }
    }

    private void OnSprintPress()
    {
        _isSprinting = true;
        _movement.IsSprinting = true;
        if (_isMoving && _movement.IsGrounded == true)
        {
            StartSprinting?.Invoke();
            //Debug.Log("Started Sprinting");
        }
    }

    private void OnSprintRelease()
    {
        _isSprinting = false;
        _movement.IsSprinting = false;
        if (_isMoving && _movement.IsGrounded == true)
        {
            StartRunning?.Invoke();
            //Debug.Log("Started Running");
        }
    }

    private void SwitchAbilities()
    {
        _swap.SwitchAbilities();
    }
}
