using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{

    public event Action Fell = delegate { };
    public event Action Jumped = delegate { };
    public event Action Landed = delegate { };

    [Header("References")]
    [SerializeField] CharacterController _controller = null;
    [SerializeField] Transform _camTransform = null;
    [SerializeField] GroundDetector _groundDetector = null;
    [Header("Movement")]
    [SerializeField] float _turnSmoothTime = 0.1f;
    [SerializeField] float _moveSpeed = 6f;
    [SerializeField] float _sprintSpeed = 45f;
    [SerializeField] float _jumpSpeed = 10f;
    [SerializeField] float _gravity = 9.8f;
    [SerializeField] float _vertSpeedCap = 60f;

    Vector3 _directionToMove;
    float _turnSmoothVelocity;
    [SerializeField] float _vertSpeed = 0;
    [SerializeField] bool _isGrounded = false;
    [SerializeField] bool _justJumped = false;
    [SerializeField] bool _isSprinting = false;
    
    public bool IsGrounded { get { return _isGrounded; } }
    public bool IsSprinting { set { _isSprinting = value; } }
    
    private void OnEnable()
    {
        _groundDetector.GroundDetected += OnGroundDetected;
        _groundDetector.GroundVanished += OnGroundVanished;
    }

    private void Start()
    {
        _vertSpeed -= _gravity;
        VertMovement();
    }

    private void FixedUpdate()
    {
        if(_directionToMove.magnitude >= 0.1f)
        {
            TurnAndHorzMove();
        }
        ApplyGravity();
        VertMovement();
    }

    private void TurnAndHorzMove()
    {
        float _targetAngle = Mathf.Atan2(_directionToMove.x, _directionToMove.z) * Mathf.Rad2Deg
                    + _camTransform.eulerAngles.y;
        float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle,
                ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, _angle, 0);
        Vector3 _moveDirection = Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;
        if(_isSprinting == false)
        {
            _controller.Move(_moveDirection.normalized * _moveSpeed * Time.deltaTime);
        }
        else
        {
            _controller.Move(_moveDirection.normalized * _sprintSpeed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if(_isGrounded == false)
        {
            _vertSpeed -= _gravity;
        }
    }

    private void VertMovement()
    {
            _vertSpeed = Mathf.Clamp(_vertSpeed, -1 * _vertSpeedCap, _vertSpeedCap);
        _controller.Move(Vector3.up * _vertSpeed * Time.deltaTime);
    }

    public void PrepareToMove(Vector3 _direction)
    {
        _directionToMove = _direction;
    }
    public void PrepareJump()
    {
        if (_isGrounded)
        {
            _vertSpeed += _jumpSpeed;
            _justJumped = true;
        }
    }

    private void OnGroundDetected()
    {
        _vertSpeed = 0;
        if(_isGrounded == false)
        {
            Landed?.Invoke();
            Debug.Log("Landed");
        }
        _isGrounded = true;
    }

    private void OnGroundVanished()
    {
        _isGrounded = false;
        CheckIfJustJumped();
    }

    private void CheckIfJustJumped()
    {
        if (_justJumped)
        {
            Jumped?.Invoke();
            Debug.Log("Jumped");
            _justJumped = false;
        }
        else
        {
            Fell.Invoke();
            Debug.Log("Fell");
        }
    }
}
