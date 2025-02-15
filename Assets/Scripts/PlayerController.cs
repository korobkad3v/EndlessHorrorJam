using System.Xml;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Cam")]
    public PlayerCam cam;
    
    [Header("Movement")]
    private float speed = 0f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float airSpeed = 0f;
    public float stamina = 100f;
    public float staminaDrainRate = 10f;

    Vector3 moveDirection = Vector3.zero;
    public Transform orientation;
    public float standartDrag = 5f;
    public float standartDragAir = 0.25f;

    private bool isSprinting = false;

    [Header("Crouch")]
    public float crouchYScale;
    private float startYScale;
    private bool isCrouching;

    [Header("Jump")]
    public float jumpForce = 7.0f;
    public float jumpCooldown = 0.5f;
    public float airMultiplier = 0.5f;
    private bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask groundMask;
    
    private bool isGrounded = true;

    [HideInInspector]
    public Rigidbody rb;

    [Header("Movement State")]
    public MovementState movementState;
    public enum MovementState 
    { 
        walk,
        sprint,
        crouch,
        air
    };
    private float stepTimer = 0f;
    public float stepInterval = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;
    }

    void OnMove(InputValue inputValue) 
    {
        Vector2 inputVector = inputValue.Get<Vector2>();
        moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;
       
    }

    public void OnSprint(InputValue inputValue) 
    {
        if (inputValue.isPressed && !isSprinting && movementState == MovementState.walk && stamina > 0) 
        {
            isSprinting = true;
        }
        else if (inputValue.isPressed && isSprinting)
        {
            isSprinting = false;
        }
    }

    void OnCrouch(InputValue inputValue)
    {
        if (inputValue.isPressed && !isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            isCrouching = true;
        }
        else if (inputValue.isPressed && isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            isCrouching = false;
        }
    }

    void OnJump(InputValue inputValue) 
    {
        if (readyToJump && isGrounded && stamina > 10f)
        {
            readyToJump = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            stamina = stamina - 10f;
            Invoke(nameof(ResetJump), jumpCooldown);
            AudioManager.Instance.Play("jump");
        }        
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void MovementStateHandler() 
    {
        if (isGrounded && isSprinting) 
        {
            movementState = MovementState.sprint;
            speed = sprintSpeed;
            rb.linearDamping = standartDrag;
            cam.changeFOV(cam.FOV * 1.5f, 2f);
        }
        else if (isGrounded && !isSprinting) 
        {
            movementState = MovementState.walk;
            speed = walkSpeed;
            rb.linearDamping = standartDrag;
            cam.changeFOV(cam.FOV * 1f);
        }
        else if (isGrounded && isCrouching) 
        {
            movementState = MovementState.crouch;
            speed = crouchSpeed;
            rb.linearDamping = standartDrag;
            cam.changeFOV(cam.FOV * 1f);
        }
        else 
        {
            movementState = MovementState.air;
            speed = airSpeed;
            rb.linearDamping = standartDragAir;
            cam.changeFOV(cam.FOV * 1f);
        }
    }

    void Update() 
    {
        SpeedControl();
        MovementStateHandler();
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
        Stamina();
        playFootstepSound();
    }

    void Stamina()
    {
        if (isSprinting && rb.linearVelocity.magnitude > 1f && stamina > 0) 
        {
            stamina -= staminaDrainRate * Time.deltaTime;
        }

        if (stamina <= 0) 
        {
            isSprinting = false;
        }

        if (!isSprinting && stamina < 100f) 
        {
            stamina += staminaDrainRate / 15 * Time.deltaTime;
        }
    }

    void FixedUpdate() 
    {
        if (movementState == MovementState.sprint || movementState == MovementState.walk)
        {
            rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);
        }
        else 
        {
            rb.AddForce(moveDirection.normalized * speed * airMultiplier, ForceMode.Force);
        }   
    }

    private void playFootstepSound()
    {
        stepTimer -= Time.deltaTime;
        if (movementState == MovementState.walk && stepTimer <= 0f && rb.linearVelocity.magnitude > 1f) 
        {
            AudioManager.Instance.Play("footstep");
            stepTimer = stepInterval;
        }
        else if (movementState == MovementState.sprint && stepTimer <= 0f && rb.linearVelocity.magnitude > 1f)
        {
            AudioManager.Instance.Play("footstep_sprint");
            stepTimer = stepInterval / 2f;
        }
    }


}
