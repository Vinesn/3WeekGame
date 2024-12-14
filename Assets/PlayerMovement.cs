using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 3.0f;
    [SerializeField] Transform movePoint;
    Vector2 movementInput;
    PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        movePoint.parent = null;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (!(Mathf.Abs(movementInput.x) != 0 && Mathf.Abs(movementInput.y) != 0))
            {
                movePoint.position += new Vector3(movementInput.x, movementInput.y, 0f);
            }
            else
            {
                movePoint.position += new Vector3(0f, movementInput.y, 0f);
            }
        }
    }
    void PlayerInput()
    {
        movementInput = playerControls.Player.Move.ReadValue<Vector2>();
    }
}
