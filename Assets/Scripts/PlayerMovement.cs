using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    Vector2 moveInput;
    bool jumpInput;

    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float baseJumpForce = 3f;

    Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

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
        isGrounded = IsGrounded();
        CheckInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();

        if (jumpInput && isGrounded)
        {
            Jump();
        }
    }

    void CheckInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        jumpInput = jumpAction.IsPressed();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * baseJumpForce, ForceMode2D.Impulse);
    }
}
