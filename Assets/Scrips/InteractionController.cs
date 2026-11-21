using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void interact();
    void onHover();
    void exitHover();
}

public class InteractionController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference interactAction;

    public Camera camera;
    public float distance = 30f;
    public Color hoverColor = Color.white;

    private IInteractable focus;
    private Renderer r;
    private Color originalColor;

    void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
            interactAction.action.performed += OnInteract;
            Debug.Log("Podpięto akcję: " + interactAction.action.name);
        }
    }

    void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteract;
            interactAction.action.Disable();
            Debug.Log("Wyłączono akcję w OnDisable na obiekcie: " + gameObject.name);
        }
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interakcja wykonana!");

        if (focus != null)
        {
            focus.interact();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (camera == null) return;

        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, distance) && hit.collider.TryGetComponent(out IInteractable i))
        {
            if (focus != i)
            {
                Clear();
                focus = i;
                focus.onHover();

                r = hit.collider.GetComponent<Renderer>();
                if(r)
                {
                    originalColor = r.material.color;
                    r.material.color = hoverColor;
                }
            }
        }
        else
        {
            Clear();
        }
    }

    void Clear()
    {
        if (focus != null) focus.exitHover();
        if (r) r.material.color = originalColor;
        focus = null;
        r = null;
    }
}
