using UnityEngine;
using UnityEngine.InputSystem;

public class Move2 : MonoBehaviour
{
    private NIS inputActions;

    [Header("Configuración")]
    public float speed = 5f;
    public float fuerzaSalto = 5f;

    [Header("Rotación")]
    public float rotationSpeed = 720f;

    [Header("Control Scheme")]
    public string controlScheme = "Keyboard_arrows";

    [Header("Cámara")]
    public Transform cameraTransform;

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
    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        inputActions = new NIS();

        inputActions.bindingMask =
            InputBinding.MaskByGroup(controlScheme);

        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;

            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
        }

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        rb.interpolation =
            RigidbodyInterpolation.Interpolate;

        rb.collisionDetectionMode =
            CollisionDetectionMode.Continuous;

        rb.angularDamping = 100f;
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
    }

    private void FixedUpdate()
    {
        Vector3 movement = GetCameraRelativeMovement();

        movement = Vector3.ClampMagnitude(
            movement,
            1f
        );

        Vector3 velocity = rb.linearVelocity;

        if (movement.sqrMagnitude > 0.001f)
        {
            velocity.x = movement.x * speed;
            velocity.z = movement.z * speed;
        }
        else
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }

        rb.linearVelocity = velocity;

        // Evitar rotación provocada por colisiones.
        rb.angularVelocity = Vector3.zero;

        if (movement.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(
                    movement,
                    Vector3.up
                );

            Quaternion newRotation =
                Quaternion.RotateTowards(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed *
                    Time.fixedDeltaTime
                );

            rb.MoveRotation(newRotation);
        }

        rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraRelativeMovement()
    {
        if (cameraTransform == null)
        {
            return new Vector3(
                moveInput.x,
                0f,
                moveInput.y
            );
        }

        Vector3 cameraForward =
            cameraTransform.forward;

        Vector3 cameraRight =
            cameraTransform.right;

        // Ignorar la inclinación vertical de la cámara.
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement =
            cameraRight * moveInput.x +
            cameraForward * moveInput.y;

        return movement;
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

        Vector3 groundCheckPosition =
            new Vector3(
                bounds.center.x,
                bounds.min.y + groundCheckDistance,
                bounds.center.z
            );

        isGrounded =
            Physics.CheckSphere(
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

        float movementAmount =
            moveInput.magnitude;

        animator.SetFloat(
            "Speed",
            movementAmount
        );

        animator.SetBool(
            "Grounded",
            isGrounded
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCollider == null)
        {
            playerCollider =
                GetComponent<Collider>();
        }

        if (playerCollider == null)
            return;

        Bounds bounds =
            playerCollider.bounds;

        Vector3 groundCheckPosition =
            new Vector3(
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