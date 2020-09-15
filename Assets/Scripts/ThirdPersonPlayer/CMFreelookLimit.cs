using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CMFreelookLimit : MonoBehaviour
{
    [SerializeField] float _camScrollSpeed = .2f;
    [Header("Free Cam Constraintes")]
    [SerializeField] float _camTopMinRadius = 3f;
    [SerializeField] float _camTopMaxRadius = 22;
    [SerializeField] float _camMidMinRadius = 8f;
    [SerializeField] float _camMidMaxRadius = 27;
    [SerializeField] float _camBottomMinRadius = 2f;
    [SerializeField] float _camBottomMaxRadius = 21;

    private CinemachineFreeLook _freelook;

    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        _freelook = GetComponent<CinemachineFreeLook>();
    }
    //Thank the Unity forums, 'cause I wasn't figuring this out on my own
    //Based off code by user WarpZone 

    private void FixedUpdate()
    {
        MouseScrollZoom();
    }

    private void MouseScrollZoom()
    {
        //Moves camera in and out by changing orbit radii
        //0 - Top Orbit, 1 - Mid Orbit, 2 - Bottom Orbit
        float _scrollAmount = Input.GetAxis("Mouse ScrollWheel") * _camScrollSpeed;
        _freelook.m_Orbits[0].m_Radius -= _scrollAmount;
        _freelook.m_Orbits[1].m_Radius -= _scrollAmount;
        _freelook.m_Orbits[2].m_Radius -= _scrollAmount;
        //Clamping values to avoid clipping into player model
        _freelook.m_Orbits[0].m_Radius = Mathf.Clamp(_freelook.m_Orbits[0].m_Radius, 
                                                    _camTopMinRadius, _camTopMaxRadius);
        _freelook.m_Orbits[1].m_Radius = Mathf.Clamp(_freelook.m_Orbits[1].m_Radius,
                                                    _camMidMinRadius, _camMidMaxRadius);
        _freelook.m_Orbits[2].m_Radius = Mathf.Clamp(_freelook.m_Orbits[2].m_Radius,
                                                    _camBottomMinRadius, _camBottomMaxRadius);
    }

    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
        return UnityEngine.Input.GetAxis(axisName);
    }
}
