using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region VARIABLES
    InputAction moveAction;
    InputAction jumpAction;
<<<<<<< Updated upstream
    Vector2 moveInput;
    bool jumpInput;

    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float baseJumpForce = 3f;

=======
>>>>>>> Stashed changes
    Rigidbody2D rb;

    [Header("Inputs")]
    Vector2 moveInput;
    bool jumpInputPress;
    bool jumpInputUp;
    bool jumpInputDown;

    [SerializeField] bool isJumping;
    [SerializeField] bool isJumpCut;
    bool isJumpButtonReleased = true;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float acceleration = 7f;
    [SerializeField] float decceleration = 7f;
    [Range(0f, 2f)] [SerializeField] float velPower = 0.9f;
    [SerializeField] float frictionAmount;
    [SerializeField] float velocityYSpeedLimit = 30f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 3f;
    [SerializeField] float jumpCooldownTime = 0.5f;
    [Range(0f, 1f)][SerializeField] float jumpCutMultiplier = 0.3f;
    [SerializeField] float jumpCoyoteTime = 0.2f;
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float jumpHangTimeTreshold = 0.2f;
    [SerializeField] float jumpHangMultiplier = 0.2f;


    float lastGroundedTime;
    float lastJumpTime;
    float lastJumpCooldown;

    [Header("Gravity")]
    [SerializeField] float gravityScale = 4f;
    [SerializeField] float fallGravityMultiplier = 1f;

    [Header("Ground Checker")]
    [SerializeField] Transform groundCheck;
<<<<<<< Updated upstream
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;
=======
    [SerializeField] Vector2 groundCheckSize = new Vector2(1f, 0.01f);
    [SerializeField] LayerMask groundLayer;

    #endregion
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
        isGrounded = IsGrounded();
        CheckInput();
=======
        CheckInput();
        InputHandler();
        
        if (!isJumping)
        {
            if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer))
            {
                lastGroundedTime = jumpCoyoteTime;
                isJumping = false;
            }
        }

        if (isJumping && rb.linearVelocity.y < 0)
        {
            isJumping = false;
        }

        if (lastGroundedTime > 0 && !isJumping)
        {
            isJumpCut = false;
        }

        if (jumpInputUp)
        {
            isJumpCut = true;
            isJumpButtonReleased = true;
        }

        #region TIMERY
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
        lastJumpCooldown -= Time.deltaTime;
        #endregion
>>>>>>> Stashed changes
    }

    private void FixedUpdate()
    {
<<<<<<< Updated upstream
        ApplyMovement();

        if (jumpInput && isGrounded)
        {
            Jump();
        }
=======
        AdjustGravity();
        HandleFriction();

        Run();

        if (CanJump() && lastJumpTime > 0)
        {
            isJumping = true;
            isJumpCut = false;
            Jump();
        }

        if (CanJumpCut() && isJumpCut)
        {
            rb.AddForce(Vector2.down * rb.linearVelocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
>>>>>>> Stashed changes
    }

    #region INPUT
    void CheckInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        jumpInputPress = jumpAction.IsPressed();
        jumpInputUp = jumpAction.WasReleasedThisFrame();
        jumpInputDown = jumpAction.WasPressedThisFrame();
    }

    void InputHandler()
    {
<<<<<<< Updated upstream
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
=======
        if (jumpInputDown)
        {
            OnJumpInputDown();
        }
        if (jumpInputUp)
        {
            OnJumpInputUp();
        }
>>>>>>> Stashed changes
    }

    void OnJumpInputDown()
    {
        lastJumpTime = jumpBufferTime;
    }
<<<<<<< Updated upstream

    private void Jump()
    {
        rb.AddForce(Vector2.up * baseJumpForce, ForceMode2D.Impulse);
=======
    void OnJumpInputUp()
    {
        if (CanJumpCut())
        {
            isJumpCut = true;
        }
    }
    #endregion

    void Run()
    {
        float targetSpeed = moveInput.x * moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.1f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right);
    }

    private void Jump()
    {
        lastJumpTime = 0;
        lastGroundedTime = 0;
        lastJumpCooldown = jumpCooldownTime;
        isJumpButtonReleased = false;

        float force = jumpForce;
        if (rb.linearVelocity.y < 0)
        {
            force -= rb.linearVelocity.y;
        }

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

>>>>>>> Stashed changes
    }

    private void AdjustGravity()
    {
        if (isJumping && rb.linearVelocity.y < jumpHangTimeTreshold)
        {
            rb.gravityScale = gravityScale * jumpHangMultiplier;
        }
        else if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -velocityYSpeedLimit));
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    void HandleFriction()
    {
        if (moveInput == Vector2.zero && lastGroundedTime > 0)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocityX), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.linearVelocityX);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    #region CHECKERY
    bool CanJumpCut()
    {
        return (isJumping && rb.linearVelocity.y > 0);
    }

    bool CanJump()
    {
        return (lastGroundedTime > 0 && !isJumping && isJumpButtonReleased && lastJumpCooldown < 0);
    }
    #endregion

    #region DEBUG
    // GROUND HITBOX DRAW/CHECK
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    //}
    #endregion
}