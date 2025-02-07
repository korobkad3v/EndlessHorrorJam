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

    public Transform orientation;

    [Header("Animation")]
    public DPad dPad;
    public float xRotation_dPad = 0f;
    public float zRotation_dPad = 0f;
    public float magnitudeThreshold = 0.1f;

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

    void Update()
    {
        dPad.RotateToZero();
    }
}
