using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };

    [SerializeField] CharacterController _controller;
    [SerializeField] Transform _camTransform;
    [SerializeField] float _speed = 6f;
    [SerializeField] float _turnSmoothTime = 0.1f;
    
    float _turnSmoothVelocity;
    bool _isMoving = false;

    private float _horizontal, _vertical;
    private Vector3 _direction;

    private void Start()
    {
        Idle?.Invoke();
    }

    private void Update()
    {
        ProcessInput();
        if (_direction.magnitude >= 0.1f)
        {
            CheckIfStartedMoving();
            TurnAndMove();
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
    }

    private void CheckIfStartedMoving()
    {
        if (_isMoving == false)
        {
            StartRunning?.Invoke();
            Debug.Log("Started Running");
        }
        _isMoving = true;
    }

    private void CheckIfStoppedMoving()
    {
        if (_isMoving == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped Running");
        }
        _isMoving = false;
    }

    private void TurnAndMove()
    {
        float _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg
                    + _camTransform.eulerAngles.y;
        float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle,
                ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, _angle, 0);
        Vector3 _moveDirection = Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;
        _controller.Move(_moveDirection.normalized * _speed * Time.deltaTime);
    }
}
