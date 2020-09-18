using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundDetector : MonoBehaviour
{
    public event Action GroundDetected = delegate { };
    public event Action GroundVanished = delegate { };

    [SerializeField] float _detectLength = 0.25f;
    [SerializeField] LayerMask _groundLayer = 0;

    private bool _isGrounded = false;
    RaycastHit _hit;

    private void Update()
    {
        DetectGround();
    }

    private void DetectGround()
    {
        if(Physics.Raycast(transform.position, Vector3.down, _detectLength, _groundLayer))
        {
            if(_isGrounded == false)
            {
                //Debug.Log("Ground Detected");
                GroundDetected?.Invoke();
                _isGrounded = true;
            }
        }
        else if (_isGrounded == true)
        {
            //Debug.Log("GroundVanished");
            GroundVanished?.Invoke();
            _isGrounded = false;
        }
    }

    public bool CheckGround(float _distance)
    {
        if (Physics.Raycast(transform.position, Vector3.down, _distance, _groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
