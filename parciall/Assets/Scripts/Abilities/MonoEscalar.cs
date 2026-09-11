using UnityEngine;
using UnityEngine.InputSystem;

public class MonoEscalar : MonoBehaviour
{
    [Header("Configuración de escalada")]
    public float velocidadEscalada = 3f;

    [Header("Control Scheme")]
    public string controlScheme = "Keyboard_WASD";

    [Header("Corrección al escalar")]
    public float ajusteAltura = 0.02f;

    private NIS inputActions;
    private Rigidbody rb;

    private bool enEscalera = false;
    private bool escalando = false;

    // Escalera actual
    private Transform escaleraActual;

    // Movimiento normal
    private MovePlayer movimientoNormal;

    // Movimiento recibido desde NIS
    private Vector2 moveInput;

    private void Awake()
    {
        inputActions = new NIS();

        inputActions.bindingMask =
            InputBinding.MaskByGroup(controlScheme);

        rb = GetComponent<Rigidbody>();

        movimientoNormal = GetComponent<MovePlayer>();
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

    // ==========================================
    // MOVIMIENTO
    // ==========================================

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // ==========================================
    // HABILIDAD DE ESCALAR
    // ==========================================

    private void OnAbility(InputAction.CallbackContext context)
    {
        if (!enEscalera)
        {
            return;
        }

        if (escalando)
        {
            DejarDeEscalar();
        }
        else
        {
            ComenzarAEscalar();
        }
    }

    // ==========================================
    // COMENZAR A ESCALAR
    // ==========================================

    private void ComenzarAEscalar()
    {
        if (!enEscalera)
            return;

        escalando = true;

        Debug.Log("🐒 COMENZÓ A ESCALAR");

        // Desactivar movimiento normal
        if (movimientoNormal != null)
        {
            movimientoNormal.enabled = false;
        }

        // Quitar gravedad
        if (rb != null)
        {
            rb.useGravity = false;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // ==========================================
        // ORIENTAR SEGÚN LA ESCALERA
        // ==========================================

        if (escaleraActual != null)
        {
            Vector3 rotacionEscalera =
                escaleraActual.eulerAngles;

            transform.rotation =
                Quaternion.Euler(
                    -90f,
                    rotacionEscalera.y,
                    rotacionEscalera.z
                );

            Debug.Log(
                "Mono orientado según escalera. Y = " +
                rotacionEscalera.y
            );
        }

        Physics.SyncTransforms();

        // ==========================================
        // CORRECCIÓN DE ALTURA
        // ==========================================

        Collider colliderMono =
            GetComponent<Collider>();

        if (colliderMono != null)
        {
            float parteInferior =
                colliderMono.bounds.min.y;

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

    // ==========================================
    // MOVIMIENTO AL ESCALAR
    // ==========================================

    private void Update()
    {
        if (!escalando)
        {
            return;
        }

        /*
         * W = subir
         * S = bajar
         *
         * A y D no se utilizan.
         */

        float movimientoVertical =
            moveInput.y;

        Vector3 movimiento =
            new Vector3(
                0f,
                movimientoVertical,
                0f
            );

        transform.Translate(
            movimiento *
            velocidadEscalada *
            Time.deltaTime,
            Space.World
        );
    }

    // ==========================================
    // SALIR POR ARRIBA
    // ==========================================

    public void SalirPorArriba(Transform puntoSalida)
    {
        if (!escalando)
            return;

        // ==========================================
        // COLOCAR AL MONO EN LA PLATAFORMA
        // ==========================================

        if (puntoSalida != null)
        {
            transform.position =
                puntoSalida.position;

            // La rotación del PuntoSalida
            // determina hacia dónde queda mirando.
            transform.rotation =
                puntoSalida.rotation;

            Debug.Log(
                "🐒 Mono colocado en el PuntoSalida"
            );
        }
        else
        {
            Debug.LogWarning(
                "⚠️ No se asignó PuntoSalida"
            );
        }

        // ==========================================
        // TERMINAR ESCALADA
        // ==========================================

        escalando = false;
        enEscalera = false;

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

        escaleraActual = null;

        Physics.SyncTransforms();

        Debug.Log(
            "🐒 ESCALADA TERMINADA"
        );
    }

    // ==========================================
    // DEJAR DE ESCALAR MANUALMENTE
    // ==========================================

    private void DejarDeEscalar()
    {
        escalando = false;

        Debug.Log(
            "🐒 El mono dejó de escalar"
        );

        if (movimientoNormal != null)
        {
            movimientoNormal.enabled = true;
        }

        if (rb != null)
        {
            rb.useGravity = true;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // ==========================================
    // DETECTAR ENTRADA A LA ESCALERA
    // ==========================================

    private void OnTriggerEnter(Collider other)
    {
        if (!CompareTag("Mono"))
            return;

        if (!other.CompareTag("Escalera"))
            return;

        enEscalera = true;

        escaleraActual =
            other.transform;

        Debug.Log(
            "🐒 El Mono detectó una escalera"
        );

        Debug.Log(
            "Rotación de la escalera: " +
            escaleraActual.eulerAngles
        );
    }

    // ==========================================
    // DETECTAR SALIDA DE LA ESCALERA
    // ==========================================

    private void OnTriggerExit(Collider other)
    {
        if (!CompareTag("Mono"))
            return;

        if (!other.CompareTag("Escalera"))
            return;

        /*
         * Si está escalando NO detenemos
         * la escalada.
         *
         * El FinalEscalera se encargará
         * de terminarla.
         */

        if (!escalando)
        {
            enEscalera = false;

            escaleraActual = null;

            Debug.Log(
                "🐒 El Mono salió de la escalera"
            );
        }
    }
}