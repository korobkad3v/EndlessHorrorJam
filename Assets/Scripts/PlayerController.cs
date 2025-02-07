using System.Xml;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [Header("Movement")]
    public float speed = 5.0f;
    public float jumpForce = 7.0f;
    Vector3 moveDirection = Vector3.zero;
    public Transform orientation;

    public float standardDrag = 5f;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask groundMask;
    private bool isJumping = false;
    private bool isGrounded = true;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void OnMove(InputValue inputValue) 
    {
        Vector2 inputVector = inputValue.Get<Vector2>();
        moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;
    }

    void OnJump(InputValue inputValue) 
    {
        if (isGrounded && !isJumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isJumping = true;
        }
    }

    void Update() 
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if (isGrounded)
        {
            rb.linearDamping = standardDrag;
        }
        else
        {
            rb.linearDamping = 0f;
        }
    }

    void FixedUpdate() 
    {
        rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);
    }
}
