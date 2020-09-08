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

    RaycastHit _hit;

    private void Update()
    {
        DetectGround();
    }

    private void DetectGround()
    {
        if(Physics.Raycast(transform.position, Vector3.down, _detectLength, _groundLayer))
        {
            GroundDetected?.Invoke();
        }
        else
        {
            GroundVanished?.Invoke();
        }
    }
}
