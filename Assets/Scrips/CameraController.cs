using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform head;     
    public Transform _camera;
    public Transform hand;
    public float cameraSensitivity = 0.15f;
    public float cameraAcceleration = 2f;

    [Header("Input Actions")]
    public InputActionReference lookAction;

    private float pitch;
    private float yaw;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable()
    {
        if (lookAction != null) lookAction.action.Enable();
    }

    void OnDisable()
    {
        if (lookAction != null) lookAction.action.Disable();
    }

    void LateUpdate()
    {
        Vector2 look = lookAction != null ? lookAction.action.ReadValue<Vector2>() : Vector2.zero;

        yaw += look.x * cameraSensitivity;
        pitch = Mathf.Clamp(pitch + look.y * cameraSensitivity, -89f, 89f);

        if (hand != null)
            hand.localRotation = Quaternion.Euler(-pitch, yaw, 0f); // kept from your original

        head.localRotation = Quaternion.Lerp(head.localRotation, Quaternion.Euler(0f, yaw, 0f), cameraAcceleration * Time.deltaTime);
        _camera.localRotation = Quaternion.Lerp(_camera.localRotation, Quaternion.Euler(-pitch, 0f, 0f), cameraAcceleration * Time.deltaTime);


    }
}