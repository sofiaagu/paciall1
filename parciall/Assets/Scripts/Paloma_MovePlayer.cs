using UnityEngine;
using UnityEngine.InputSystem;

public class Paloma_MovePlayer : MonoBehaviour
{
    private NIS inputActions;

    [Header("Configuración")]
    public float speed = 5f;
    public float fuerzaSalto = 5f;

    [Header("Control Scheme")]
    public string controlScheme = "Keyboard_arrows";

    private Vector2 moveInput;
    private Rigidbody rb;

    private void Awake()
    {
        inputActions = new NIS();

        // Este jugador solamente utilizará los bindings
        // pertenecientes al Control Scheme seleccionado.
        inputActions.bindingMask =
            InputBinding.MaskByGroup(controlScheme);

        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext value)
    {
        moveInput = value.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log(gameObject.name + " saltó");

        rb.AddForce(
            Vector3.up * fuerzaSalto,
            ForceMode.Impulse
        );
    }

    private void Update()
    {
        Vector3 movement = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );

        transform.Translate(
            movement * speed * Time.deltaTime
        );
    }
}