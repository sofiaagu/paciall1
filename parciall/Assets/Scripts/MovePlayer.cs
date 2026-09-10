using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    private NIS inputActions;

    [Header("Configuración")]
    public float speed = 5f;
    public float fuerzaSalto = 5f;

    [Header("Rotación")]
    public float rotationSpeed = 720f;

    [Header("Control Scheme")]
    public string controlScheme = "Keyboard_arrows";

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.25f;
    public float groundCheckDistance = 0.08f;

    [Header("Animaciones")]
    public Animator animator;

    private Vector2 moveInput;
    private Rigidbody rb;
    private Collider playerCollider;

    private bool isGrounded;

    private void Awake()
    {
        inputActions = new NIS();

        // Utiliza únicamente los controles del personaje.
        inputActions.bindingMask =
            InputBinding.MaskByGroup(controlScheme);

        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Evita que el personaje se tumbe por las colisiones.
        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
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
        if (!isGrounded)
            return;

        // Reinicia la velocidad vertical para evitar
        // acumulación de fuerza.
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        rb.AddForce(
            Vector3.up * fuerzaSalto,
            ForceMode.Impulse
        );

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }

        Debug.Log(gameObject.name + " saltó");
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );

        movement = movement.normalized;

        if (movement.magnitude > 0.01f)
        {
            // Movimiento físico
            Vector3 newPosition =
                rb.position +
                movement * speed * Time.fixedDeltaTime;

            rb.MovePosition(newPosition);

            // Rotación hacia la dirección de movimiento
            Quaternion targetRotation =
                Quaternion.LookRotation(movement);

            rb.MoveRotation(
                Quaternion.RotateTowards(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                )
            );
        }
    }

    private void Update()
    {
        CheckGround();
        UpdateAnimations();
    }

    private void CheckGround()
    {
        if (playerCollider == null)
            return;

        Bounds bounds = playerCollider.bounds;

        Vector3 groundCheckPosition = new Vector3(
            bounds.center.x,
            bounds.min.y + groundCheckDistance,
            bounds.center.z
        );

        isGrounded = Physics.CheckSphere(
            groundCheckPosition,
            groundCheckRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );
    }

    private void UpdateAnimations()
    {
        if (animator == null)
            return;

        float movementAmount = moveInput.magnitude;

        animator.SetFloat("Speed", movementAmount);
        animator.SetBool("Grounded", isGrounded);
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCollider == null)
            playerCollider = GetComponent<Collider>();

        if (playerCollider == null)
            return;

        Bounds bounds = playerCollider.bounds;

        Vector3 groundCheckPosition = new Vector3(
            bounds.center.x,
            bounds.min.y + groundCheckDistance,
            bounds.center.z
        );

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            groundCheckPosition,
            groundCheckRadius
        );
    }
}