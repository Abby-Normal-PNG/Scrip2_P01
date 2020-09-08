using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonInput : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };

    [SerializeField] ThirdPersonMovement _movement = null;

    bool _isMoving = false;
    private float _horizontal, _vertical;
    private Vector3 _direction;

    private void OnEnable()
    {
        _movement.Landed += OnLanded;
    }

    private void OnDisable()
    {
        _movement.Landed += OnLanded;
    }

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

    private void OnLanded()
    {
        if (_isMoving)
        {
            StartRunning?.Invoke();
            Debug.Log("Land & Run");
        }
        else
        {
            Idle?.Invoke();
            Debug.Log("Land & Idle");
        }
    }

}
