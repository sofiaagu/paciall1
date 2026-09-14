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
    public float velocidadBajada = 5f;

    [Header("Animación")]
    public Animator animator;

    private bool isFlying;

    // Altura desde donde despega
    private float alturaInicial;

    private void Awake()
    {
        inputActions = new NIS();

        // La paloma utiliza las flechas
        inputActions.bindingMask =
            InputBinding.MaskByGroup("Keyboard_arrows");

        movePlayer = GetComponent<MovePlayer>();
        rb = GetComponent<Rigidbody>();

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
        // No puede activar el vuelo mientras ya está volando
        if (isFlying)
            return;

        // Solo puede despegar desde el suelo
        if (!movePlayer.IsGrounded)
            return;

        StartCoroutine(Fly());
    }

    private IEnumerator Fly()
    {
        isFlying = true;

        // ------------------------------------------
        // GUARDAR ALTURA INICIAL
        // ------------------------------------------

        alturaInicial = rb.position.y;

        float alturaObjetivo =
            alturaInicial + alturaVuelo;

        // ------------------------------------------
        // PREPARAR RIGIDBODY
        // ------------------------------------------

        rb.useGravity = false;

        // El movimiento vertical lo controlará
        // temporalmente esta habilidad.
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;
        rb.linearVelocity = velocity;

        // ------------------------------------------
        // ANIMACIÓN DE VUELO
        // ------------------------------------------

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
                Mathf.Clamp01(
                    tiempo / duracionSubida
                );

            float altura =
                Mathf.Lerp(
                    alturaInicial,
                    alturaObjetivo,
                    progreso
                );

            Vector3 posicion = rb.position;

            posicion.y = altura;

            rb.MovePosition(posicion);

            yield return new WaitForFixedUpdate();
        }

        // ==========================================
        // 2. MANTENERSE VOLANDO
        // ==========================================

        tiempo = 0f;

        while (tiempo < duracionVuelo)
        {
            tiempo += Time.fixedDeltaTime;

            Vector3 posicion = rb.position;

            posicion.y = alturaObjetivo;

            rb.MovePosition(posicion);

            yield return new WaitForFixedUpdate();
        }

        // ==========================================
        // 3. COMENZAR DESCENSO
        // ==========================================

        // Ahora devolvemos la gravedad.
        rb.useGravity = true;

        // Dejamos de controlar directamente la posición.
        // La física se encargará de bajar y detectar
        // correctamente el suelo/objetos.

        Vector3 velocidad = rb.linearVelocity;

        velocidad.y = -velocidadBajada;

        rb.linearVelocity = velocidad;

        // ==========================================
        // ESPERAR ATERRIZAJE
        // ==========================================

        while (!movePlayer.IsGrounded)
        {
            yield return new WaitForFixedUpdate();
        }

        // ==========================================
        // ATERRIZAJE
        // ==========================================

        Vector3 velocidadFinal = rb.linearVelocity;

        velocidadFinal.y = 0f;

        rb.linearVelocity = velocidadFinal;

        // ------------------------------------------
        // ANIMACIÓN
        // ------------------------------------------

        if (animator != null)
        {
            animator.SetBool("Flying", false);
        }

        isFlying = false;
    }
}