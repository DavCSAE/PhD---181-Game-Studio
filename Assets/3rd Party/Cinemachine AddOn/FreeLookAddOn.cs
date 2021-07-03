using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineFreeLook))]
public class FreeLookAddOn : MonoBehaviour
{
    bool canControl = true;
    [SerializeField] float mouseSpeed = 0.1f;
    [SerializeField] float gamepadSpeed = 1f;

    [Range(0f, 10f)] public float LookSpeed = 1f;
    public bool invertY = true;
    private CinemachineFreeLook _freeLookComponent;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void Start()
    {
        _freeLookComponent = GetComponent<CinemachineFreeLook>();
    }

    public void Update()
    {
        if (!canControl) return;

        // Get look input (mouse/joystick)
        Vector2 lookMovement = InputManager.Singleton.GetLookInput();

        if (invertY)
        {
            lookMovement.y = -lookMovement.y;
        }

        // Do this because X axis only contains between -180 and 180 instead of 0 and 1 like the Y axis
        lookMovement.x = lookMovement.x * 180f;

        //Ajust axis values using look speed and Time.deltaTime so the look doesn't go faster if there is more FPS
        _freeLookComponent.m_XAxis.Value += lookMovement.x * LookSpeed * Time.deltaTime;
        _freeLookComponent.m_YAxis.Value += lookMovement.y * LookSpeed * Time.deltaTime;
    }

    public void EnableCameraControl()
    {
        canControl = true;
    }

    public void DisableCameraControl()
    {
        canControl = false;
    }

    void UpdateCameraSpeed()
    {
        
    }
}