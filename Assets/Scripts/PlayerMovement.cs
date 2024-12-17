using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    Vector2 moveInput;
    bool jumpInput;

    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 3f;
    [SerializeField] float jumpTime;
    [SerializeField] float coyoteTime = 0.2f;
    
    float jumpTimeCounter;
    float coyoteTimeCounter;

    [SerializeField] float fallMultiplier;
    [SerializeField] float gravityPullMultiplier;

    Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;
    bool isJumpButtonReleased;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Player/Jump");
    }

    private void Update()
    {
        Debug.Log(coyoteTimeCounter);

        isGrounded = IsGrounded();
        CheckInput();
        CoyoteTimeHandler();
        HandleJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        AdjustGravity();
    }

    void CheckInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        jumpInput = jumpAction.IsPressed();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //Naprawiæ Hitbox, dzia³a tylko na œrodku postaci nie po rogach.
    }
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void AdjustGravity()
    {
        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (gravityPullMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void HandleJump()
    {
        if (jumpInput && coyoteTimeCounter > 0f && isJumpButtonReleased)
        {
            jumpTimeCounter = jumpTime;
            Jump();
            isJumpButtonReleased = false;
        }

        if (rb.linearVelocity.y > 0 && jumpInput)
        {
            if (jumpTimeCounter > 0)
            {
                Jump();
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (!jumpInput)
        {
            isJumpButtonReleased = true;
        }
    }
    void CoyoteTimeHandler()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else 
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = Vector2.up * jumpForce;
    }
}
