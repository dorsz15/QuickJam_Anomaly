using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    public Transform head;
    public float playerSpeed = 8.0f;
    public float jumpForce = 6.0f;

    [Header("Ground Check")]
    public LayerMask groundLayer;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpRequested;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (jumpAction != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPerformed;
        }
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (jumpAction != null)
        {
            jumpAction.action.performed -= OnJumpPerformed;
            jumpAction.action.Disable();
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpRequested = true;
    }

    void Update()
    {
        moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
    }

    void FixedUpdate()
    {
        Vector3 right = head != null ? head.right : transform.right;
        Vector3 forward = head != null ? head.forward : transform.forward;
        right.y = 0f;
        forward.y = 0f;

        Vector3 desiredMove = (right.normalized * moveInput.x + forward.normalized * moveInput.y).normalized * playerSpeed;

        rb.linearVelocity = new Vector3(desiredMove.x, rb.linearVelocity.y, desiredMove.z);

        if (jumpRequested)
        {
            jumpRequested = false;
            if (isTouchingGround())
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            }
        }
    }

    private bool isTouchingGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.CheckSphere(origin, 0.2f, groundLayer, QueryTriggerInteraction.Ignore);
    }
}