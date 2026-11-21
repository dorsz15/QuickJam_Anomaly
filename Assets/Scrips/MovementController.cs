using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    public Transform head;
    public float playerSpeed = 8.0f;

    [Header("Ground Check")]
    public LayerMask groundLayer;

    [Header("Input Actions")]
    public InputActionReference moveAction;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
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
    }

    private bool isTouchingGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.CheckSphere(origin, 0.2f, groundLayer, QueryTriggerInteraction.Ignore);
    }
}