using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PigeonAbility : MonoBehaviour
{
    private NIS inputActions;
    private MovePlayer movePlayer;
    private Rigidbody rb;

    [Header("Configuración de vuelo")]
    public float alturaVuelo = 3f;
    public float duracionSubida = 0.5f;
    public float duracionVuelo = 4f;
    public float duracionBajada = 0.5f;

    [Header("Animación")]
    public Animator animator;

    private bool isFlying;

    private void Awake()
    {
        // Crear sistema de entrada
        inputActions = new NIS();

        // La paloma utiliza las flechas del teclado
        inputActions.bindingMask =
            InputBinding.MaskByGroup("Keyboard_arrows");

        // Obtener componentes
        movePlayer = GetComponent<MovePlayer>();
        rb = GetComponent<Rigidbody>();

        // Buscar Animator automáticamente si no se asigna
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Ability.performed += OnAbility;
    }

    private void OnDisable()
    {
        inputActions.Player.Ability.performed -= OnAbility;

        inputActions.Player.Disable();
    }

    private void OnAbility(InputAction.CallbackContext context)
    {
        // No puede volver a activar el vuelo mientras ya está volando
        if (isFlying)
            return;

        // Solo puede despegar estando en el suelo
        if (!movePlayer.IsGrounded)
            return;

        StartCoroutine(Fly());
    }

    private IEnumerator Fly()
    {
        isFlying = true;

        // Desactivar gravedad mientras está volando
        rb.useGravity = false;

        // Guardar la altura desde donde despega
        float alturaInicial = rb.position.y;

        // Calcular la altura máxima
        float alturaObjetivo = alturaInicial + alturaVuelo;

        // Activar animación de vuelo
        if (animator != null)
        {
            animator.SetBool("Flying", true);
        }

        // ==========================================
        // 1. DESPEGUE
        // ==========================================

        float tiempo = 0f;

        while (tiempo < duracionSubida)
        {
            tiempo += Time.fixedDeltaTime;

            float progreso =
                Mathf.Clamp01(tiempo / duracionSubida);

            // Movimiento suave hacia arriba
            float altura =
                Mathf.Lerp(
                    alturaInicial,
                    alturaObjetivo,
                    progreso
                );

            Vector3 nuevaPosicion = rb.position;

            nuevaPosicion.y = altura;

            rb.MovePosition(nuevaPosicion);

            yield return new WaitForFixedUpdate();
        }

        // ==========================================
        // 2. MANTENERSE VOLANDO
        // ==========================================

        tiempo = 0f;

        while (tiempo < duracionVuelo)
        {
            tiempo += Time.fixedDeltaTime;

            // Mantener la paloma en la altura de vuelo
            Vector3 posicion = rb.position;

            posicion.y = alturaObjetivo;

            rb.MovePosition(posicion);

            yield return new WaitForFixedUpdate();
        }

        // ==========================================
        // 3. DESCENSO
        // ==========================================

        tiempo = 0f;

        while (tiempo < duracionBajada)
        {
            tiempo += Time.fixedDeltaTime;

            float progreso =
                Mathf.Clamp01(tiempo / duracionBajada);

            // Movimiento suave hacia abajo
            float altura =
                Mathf.Lerp(
                    alturaObjetivo,
                    alturaInicial,
                    progreso
                );

            Vector3 nuevaPosicion = rb.position;

            nuevaPosicion.y = altura;

            rb.MovePosition(nuevaPosicion);

            yield return new WaitForFixedUpdate();
        }

        // Asegurar que termine exactamente en la altura inicial
        Vector3 posicionFinal = rb.position;

        posicionFinal.y = alturaInicial;

        rb.MovePosition(posicionFinal);

        // Volver a activar la gravedad
        rb.useGravity = true;

        // Desactivar animación de vuelo
        if (animator != null)
        {
            animator.SetBool("Flying", false);
        }

        isFlying = false;
    }
}