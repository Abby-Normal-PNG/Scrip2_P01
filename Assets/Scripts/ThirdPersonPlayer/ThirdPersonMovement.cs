using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] CanvasGroup _playerUI = null;
    [Header("Movement")]
    [SerializeField] float _turnSmoothTime = 0.1f;
    [SerializeField] float _moveSpeed = 6f;
    [SerializeField] float _sprintSpeed = 45f;
    [SerializeField] float _jumpSpeed = 10f;
    [SerializeField] float _gravity = 9.8f;
    [SerializeField] float _vertSpeedCap = 60f;
    [SerializeField] float _inclineTolerance = 1f;

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

    private void OnDisable()
    {
        _groundDetector.GroundDetected -= OnGroundDetected;
        _groundDetector.GroundVanished -= OnGroundVanished;
    }

    private void Start()
    {
        _vertSpeed -= _gravity;
        VertMovement();
        OnPlayerControl();
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

    void OnPlayerControl()
    {
        _playerUI.alpha = 1;
    }

    void OnPlayerRelease()
    {
        _playerUI.alpha = 0;
    }

    private void TurnAndHorzMove()
    {
        float targetAngle = Mathf.Atan2(_directionToMove.x, _directionToMove.z) * Mathf.Rad2Deg
                    + _camTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        //If sprinting and grounded, use sprint speed
        if(_isSprinting == true && _isGrounded == true)
        {
            _controller.Move(moveDirection.normalized * _sprintSpeed * Time.deltaTime);
        }
        //If not sprinting or in the air, use normal speed
        else
        {
            _controller.Move(moveDirection.normalized * _moveSpeed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        _vertSpeed -= _gravity;
    }

    private void VertMovement()
    {
        _vertSpeed = Mathf.Clamp(_vertSpeed, -1 * _vertSpeedCap, _vertSpeedCap);
        _controller.Move(Vector3.up * _vertSpeed * Time.deltaTime);
    }

    public void PrepareToMove(Vector3 direction)
    {
        _directionToMove = direction;
    }
    public void PrepareJump()
    {
        if (_isGrounded)
        {
            _vertSpeed = _jumpSpeed;
            VertMovement();
            _justJumped = true;
        }
    }

    public void PrepareSuperJump(float superJumpSpeed)
    {
        if (_isGrounded)
        {
            _vertSpeed = superJumpSpeed;
            VertMovement();
            _justJumped = true;
        }
    }

    private void OnGroundDetected()
    {
        _vertSpeed = 0;
        if(_isGrounded == false)
        {
            Landed?.Invoke();
            //Debug.Log("Landed");
        }
        _isGrounded = true;
        _justJumped = false;
    }

    private void OnGroundVanished()
    {
        CheckIfJustJumped();
    }

    private void CheckIfJustJumped()
    {
        if (_justJumped)
        {
            Jumped?.Invoke();
            _isGrounded = false;
            //Debug.Log("Jumped");
        }
        else
        {
            CheckForRamp();
        }
    }

    private void CheckForRamp()
    {
        if (_groundDetector.CheckGround(_inclineTolerance))
        {
            ApplyGravity();
            VertMovement();
        }
        else if (_isGrounded == true)
        {
            _vertSpeed = 0;
            Fell.Invoke();
            _isGrounded = false;
            //Debug.Log("Fell");
        }
    }

    public void Knockback(Vector3 knockbackOrigin, float knockbackForce)
    {
        Vector3 _knockbackMotion = (gameObject.transform.position - knockbackOrigin) * knockbackForce;
        _controller.Move(_knockbackMotion);
    }
}
