using UnityEngine;
using UnityEngine.InputSystem;

public class MonoEscalar : MonoBehaviour
{
    [Header("Configuración de escalada")]
    public float velocidadEscalada = 3f;

    [Header("Control Scheme")]
    public string controlScheme = "Keyboard_WASD";

    private NIS inputActions;
    private Rigidbody rb;

    private bool enEscalera = false;
    private bool escalando = false;

    private MovePlayer movimientoNormal;

    private Vector2 moveInput;

    // Guardar la rotación normal
    private Quaternion rotacionNormal;


    private void Awake()
    {
        inputActions = new NIS();

        inputActions.bindingMask =
            InputBinding.MaskByGroup(controlScheme);

        rb = GetComponent<Rigidbody>();

        movimientoNormal = GetComponent<MovePlayer>();

        // Guardar rotación inicial
        rotacionNormal = transform.rotation;
    }


    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Ability.performed += OnAbility;
    }


    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Ability.performed -= OnAbility;

        inputActions.Player.Disable();
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    // =====================================================
    // E
    // =====================================================

    private void OnAbility(InputAction.CallbackContext context)
    {
        // Si NO está dentro de la escalera
        // E no hace absolutamente nada
        if (!enEscalera)
        {
            Debug.Log("E presionada, pero NO está en escalera");
            return;
        }

        // Si ya está escalando
        if (escalando)
        {
            DejarDeEscalar();
        }
        else
        {
            ComenzarAEscalar();
        }
    }


    // =====================================================
    // COMENZAR A ESCALAR
    // =====================================================

    private void ComenzarAEscalar()
    {
        if (!enEscalera)
            return;

        escalando = true;

        Debug.Log("COMENZÓ A ESCALAR");

        // Desactivar movimiento normal
        if (movimientoNormal != null)
        {
            movimientoNormal.enabled = false;
        }

        // Desactivar gravedad
        if (rb != null)
        {
            rb.useGravity = false;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // ==========================================
        // ROTACIÓN EXACTA
        // ==========================================

        Vector3 rotacionActual = transform.eulerAngles;

        transform.rotation = Quaternion.Euler(
            -90f,
            rotacionActual.y,
            rotacionActual.z
        );
    }


    // =====================================================
    // DEJAR DE ESCALAR
    // =====================================================

    private void DejarDeEscalar()
    {
        if (!escalando)
            return;

        escalando = false;

        Debug.Log("DEJÓ DE ESCALAR");

        // Detener movimiento
        moveInput = Vector2.zero;

        // Activar movimiento normal
        if (movimientoNormal != null)
        {
            movimientoNormal.enabled = true;
        }

        // Activar gravedad
        if (rb != null)
        {
            rb.useGravity = true;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // ==========================================
        // VOLVER A ROTACIÓN NORMAL
        // ==========================================

        transform.rotation = rotacionNormal;
    }


    // =====================================================
    // MOVIMIENTO DE ESCALADA
    // =====================================================

    private void Update()
    {
        if (!escalando)
            return;

        float movimientoVertical = moveInput.y;

        Vector3 movimiento = new Vector3(
            0f,
            movimientoVertical,
            0f
        );

        transform.Translate(
            movimiento * velocidadEscalada * Time.deltaTime,
            Space.World
        );
    }


    // =====================================================
    // ENTRAR A LA ESCALERA
    // =====================================================

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Escalera"))
        {
            enEscalera = true;

            // Guardar la rotación JUSTO antes de escalar
            rotacionNormal = transform.rotation;

            Debug.Log("ENTRÓ AL TRIGGER DE ESCALERA");
        }
    }


    // =====================================================
    // SALIR DE LA ESCALERA
    // =====================================================

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Escalera"))
        {
            Debug.Log("SALIÓ DEL TRIGGER DE ESCALERA");

            enEscalera = false;

            // SIEMPRE detener la escalada
            if (escalando)
            {
                DejarDeEscalar();
            }
            else
            {
                // Aunque no estuviera escalando,
                // aseguramos la rotación normal
                transform.rotation = rotacionNormal;
            }

            moveInput = Vector2.zero;
        }
    }
}