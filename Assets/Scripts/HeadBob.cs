using UnityEngine;

public class HeadBob : MonoBehaviour
{    
    
    [SerializeField, Range(0f, 5f)]
    private float bobFrequency = 1.5f;
    [SerializeField, Range(0f, 1f)]
    private float bobAmplitude = 0.05f;
    [SerializeField, Range(0f, 1f)]
    private float bobSway = 0.05f;
    [SerializeField]
    private float tiltAngle = 5f;
    [SerializeField]
    private float tiltSpeed = 5f;
    

    public PlayerController playerController;

    public float toggleSpeed = 1.0f;
    private float timer;
    private float currentTilt = 0f;
    private Vector3 initialCameraPosition;

    private void Start()
    {
        initialCameraPosition = transform.localPosition;
    }

    void Update()
    {   
        Vector3 vel = transform.InverseTransformDirection(playerController.rb.linearVelocity);
        if (playerController.movementState != PlayerController.MovementState.air && vel.magnitude > toggleSpeed) 
        {
            timer += Time.deltaTime * bobFrequency;
            float horizontalBob = Mathf.Sin(timer) * bobSway;
            float verticalBob = Mathf.Cos(timer * 2f) * bobAmplitude;

            transform.localPosition = initialCameraPosition + new Vector3(horizontalBob, verticalBob, 0f);


            float tilt = Mathf.Clamp(-vel.x * tiltAngle, -tiltAngle, tiltAngle);

            currentTilt = Mathf.Lerp(currentTilt, tilt, Time.deltaTime * tiltSpeed);
        }
        else 
        {
            timer = Mathf.Repeat(timer, Mathf.PI * 2f);
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialCameraPosition, Time.deltaTime * tiltSpeed);
            currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * tiltSpeed);
        }

        transform.localRotation = Quaternion.Euler(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y,
                currentTilt
            );
    }
}
