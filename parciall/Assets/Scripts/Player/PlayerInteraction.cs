using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private NIS inputActions;

    [Header("Control Scheme")]
    public string controlScheme = "Keyboard_arrows";

    [Header("Interacción")]
    public float interactionDistance = 2f;

    private Interactable currentInteractable;

    private void Awake()
    {
        inputActions = new NIS();

        // Utiliza únicamente los controles de este personaje.
        inputActions.bindingMask =
            InputBinding.MaskByGroup(controlScheme);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Interact.started += OnInteractStarted;
        inputActions.Player.Interact.canceled += OnInteractCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.started -= OnInteractStarted;
        inputActions.Player.Interact.canceled -= OnInteractCanceled;

        inputActions.Player.Disable();
    }

    private void OnInteractStarted(InputAction.CallbackContext context)
    {
        DetectInteraction();

        if (currentInteractable != null)
        {
            currentInteractable.Interact(this);
        }
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            currentInteractable.StopInteract(this);
        }

        currentInteractable = null;
    }

    private void DetectInteraction()
    {
        currentInteractable = null;

        // Punto desde donde sale el Raycast.
        Vector3 origin =
            transform.position + Vector3.up * 0.5f;

        // Dirección hacia donde mira el personaje.
        Vector3 direction = transform.forward;

        if (Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            interactionDistance))
        {
            currentInteractable =
                hit.collider.GetComponentInParent<Interactable>();

            if (currentInteractable != null)
            {
                Debug.Log(
                    gameObject.name +
                    " puede interactuar con: " +
                    currentInteractable.gameObject.name
                );
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin =
            transform.position + Vector3.up * 0.5f;

        Vector3 direction = transform.forward;

        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(
            origin,
            direction * interactionDistance
        );
    }
}