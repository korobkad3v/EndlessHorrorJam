using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{

    [Header("Camera")]
    public float sensX;
    public float sensY;

    public float xRotation = 0f;
    public float yRotation = 0f;

    [Range(1f, 180f)]
    public float FOV = 60f  ;

    public Transform orientation;

    [Header("Animation")]
    public DPad dPad;
    public float xRotation_dPad = 0f;
    public float zRotation_dPad = 0f;
    public float magnitudeThreshold = 0.1f;

    private Vector2 lastInputVector = Vector2.zero;
    
    public Camera cam;
    // bad code
    private int badCode = 1;
    private int badCodeCounter = 0;

    private void BadCode()
    {
        if (badCodeCounter < badCode) 
        {
            cam.enabled = true;
            badCodeCounter++;
        }
        
    }

    void Awake()
    {
        if (cam == null) 
        {
            cam = GetComponent<Camera>();
            Debug.Log("Camera assigned in Awake: " + (cam != null));
        }

        cam.fieldOfView = FOV;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void OnLook(InputValue inputValue) 
    {
        Vector2 inputVector = inputValue.Get<Vector2>();
        
        xRotation -= inputVector.y * sensX * Time.deltaTime;
        yRotation += inputVector.x * sensY * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // DPad animation    
        if (inputVector.magnitude > magnitudeThreshold)
        {
            xRotation_dPad = inputVector.y * sensX;
            zRotation_dPad = inputVector.x * sensX;

            xRotation_dPad = Mathf.Clamp(xRotation_dPad, -dPad.clampValues.x, dPad.clampValues.x);
            zRotation_dPad = Mathf.Clamp(zRotation_dPad, -dPad.clampValues.y, dPad.clampValues.y);

            if (Mathf.Abs(inputVector.normalized.x - lastInputVector.normalized.x) > magnitudeThreshold || Mathf.Abs(inputVector.normalized.y - lastInputVector.normalized.y) > magnitudeThreshold)  
            {
                AudioManager.Instance.Play("click_4");  
                  
            }

            lastInputVector = inputVector.normalized;  

        }
        else
        {
            xRotation_dPad = 0f;
            zRotation_dPad = 0f;
        }

        // Apply
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        dPad.transform.localRotation = Quaternion.Slerp(dPad.transform.localRotation, 
            Quaternion.Euler(-xRotation_dPad, 0, zRotation_dPad), 
            dPad.smoothSpeed * Time.deltaTime);
    }

    public void changeFOV(float fov, float tMod = 1)
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * tMod);
    }

    void Update()
    {
        dPad.RotateToZero();
        BadCode();
    }
}
