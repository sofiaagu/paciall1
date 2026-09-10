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

    // Referencia al movimiento normal del mono
    private MovePlayer movimientoNormal;

    // Movimiento recibido desde NIS
    private Vector2 moveInput;

    [Header("Corrección al escalar")]
    public float ajusteAltura = 0.02f;

    private void Awake()
    {
        // Crear las acciones de entrada
        inputActions = new NIS();

        // Utilizar solamente el Control Scheme seleccionado
        inputActions.bindingMask =
            InputBinding.MaskByGroup(controlScheme);

        // Obtener Rigidbody del personaje
        rb = GetComponent<Rigidbody>();

        // Obtener el script de movimiento normal
        movimientoNormal = GetComponent<MovePlayer>();
    }


    private void OnEnable()
    {
        inputActions.Player.Enable();

        // Movimiento W, A, S, D
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        // Ability = E
        inputActions.Player.Ability.performed += OnAbility;
    }


    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Ability.performed -= OnAbility;

        inputActions.Player.Disable();
    }


    // Recibe el movimiento de W, A, S y D
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    // Se ejecuta cuando presionamos E
    private void OnAbility(InputAction.CallbackContext context)
    {
        // Si NO estamos tocando una escalera,
        // E no hace nada.
        if (!enEscalera)
        {
            return;
        }

        // Si ya está escalando, deja de escalar.
        if (escalando)
        {
            DejarDeEscalar();
        }
        else
        {
            ComenzarAEscalar();
        }
    }


    // ACTIVAR ESCALADA
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
        // ROTAR EL MONO
        // ==========================================

        Vector3 rotacionActual = transform.eulerAngles;

        transform.rotation = Quaternion.Euler(
            -90f,
            rotacionActual.y,
            rotacionActual.z
        );

        // ==========================================
        // ACTUALIZAR COLLIDER
        // ==========================================

        Physics.SyncTransforms();

        // ==========================================
        // SACAR EL COLLIDER DEL PISO
        // ==========================================

        Collider colliderMono = GetComponent<Collider>();

        if (colliderMono != null)
        {
            float parteInferior = colliderMono.bounds.min.y;

            // Si la parte inferior está debajo de Y = 0
            if (parteInferior < 0f)
            {
                float cuantoEstaEnterrado =
                    -parteInferior + ajusteAltura;

                transform.position +=
                    Vector3.up * cuantoEstaEnterrado;

                Debug.Log(
                    "Mono levantado: " +
                    cuantoEstaEnterrado
                );
            }
        }
    }


    // DESACTIVAR ESCALADA
    private void DejarDeEscalar()
    {
        escalando = false;

        Debug.Log("El mono dejó de escalar");

        // Activar nuevamente movimiento normal
        if (movimientoNormal != null)
        {
            movimientoNormal.enabled = true;
        }

        // Activar nuevamente gravedad
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }


    private void Update()
    {
        // Si no está escalando,
        // este script no hace nada.
        if (!escalando)
        {
            return;
        }

        /*
         * Mientras escala:
         *
         * W = subir
         * S = bajar
         *
         * A y D no se utilizan para subir/bajar.
         */

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


    // DETECTAR CUANDO ENTRA A LA ESCALERA
    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Mono") && other.CompareTag("Escalera"))
        {
            enEscalera = true;

            Debug.Log("El Mono detectó una escalera");
        }
    }


    // DETECTAR CUANDO SALE DE LA ESCALERA
    private void OnTriggerExit(Collider other)
    {
        if (CompareTag("Mono") && other.CompareTag("Escalera"))
        {
            enEscalera = false;

            // Si estaba escalando, deja de escalar
            if (escalando)
            {
                DejarDeEscalar();
            }

            Debug.Log("El Mono salió de la escalera");
        }
    }
}