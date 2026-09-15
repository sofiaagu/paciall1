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
    public float tiempoMaximoVuelo = 5f; // Tiempo máximo permitido si no suelta la tecla
    public float velocidadBajada = 5f;

    [Header("Audio de Vuelo")]
    public AudioClip sonidoAleteo;
    [Range(0f, 1f)] public float volumenAleteo = 0.7f;
    private AudioSource emisor3D;

    [Header("Animación")]
    public Animator animator;

    private bool isFlying;
    private bool holdsAbilityKey; // Controla si la tecla sigue presionada

    private float alturaInicial;

    private void Awake()
    {
        inputActions = new NIS();

        inputActions.bindingMask =
            InputBinding.MaskByGroup("Keyboard_arrows");

        movePlayer = GetComponent<MovePlayer>();
        rb = GetComponent<Rigidbody>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Configuración del emisor 3D de audio
        emisor3D = gameObject.AddComponent<AudioSource>();
        emisor3D.clip = sonidoAleteo;
        emisor3D.loop = true;          // Aleteo continuo en vuelo
        emisor3D.spatialBlend = 1f;    // 100% 3D
        emisor3D.minDistance = 1f;
        emisor3D.maxDistance = 15f;
        emisor3D.volume = volumenAleteo;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        // Presionar la tecla
        inputActions.Player.Ability.performed += OnAbilityStarted;
        // Soltar la tecla
        inputActions.Player.Ability.canceled += OnAbilityCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Ability.performed -= OnAbilityStarted;
        inputActions.Player.Ability.canceled -= OnAbilityCanceled;

        inputActions.Player.Disable();
    }

    private void OnAbilityStarted(InputAction.CallbackContext context)
    {
        holdsAbilityKey = true;

        // No puede activar el vuelo mientras ya está volando
        if (isFlying)
            return;

        // Solo puede despegar desde el suelo
        if (!movePlayer.IsGrounded)
            return;

        StartCoroutine(Fly());
    }

    private void OnAbilityCanceled(InputAction.CallbackContext context)
    {
        holdsAbilityKey = false; // El jugador soltó la tecla
    }

    private IEnumerator Fly()
    {
        isFlying = true;

        // ------------------------------------------
        // REPRODUCIR AUDIO DE ALETEO
        // ------------------------------------------

        if (sonidoAleteo != null && emisor3D != null)
        {
            emisor3D.clip = sonidoAleteo;
            emisor3D.volume = volumenAleteo;
            emisor3D.Play();
        }

        // ------------------------------------------
        // GUARDAR ALTURA INICIAL
        // ------------------------------------------

        alturaInicial = rb.position.y;
        float alturaObjetivo = alturaInicial + alturaVuelo;

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
            // Si el jugador suelta el botón durante la subida, interrumpe el ascenso
            if (!holdsAbilityKey)
                break;

            tiempo += Time.fixedDeltaTime;

            float progreso = Mathf.Clamp01(tiempo / duracionSubida);
            float altura = Mathf.Lerp(alturaInicial, alturaObjetivo, progreso);

            Vector3 posicion = rb.position;
            posicion.y = altura;
            rb.MovePosition(posicion);

            yield return new WaitForFixedUpdate();
        }

        // ==========================================
        // 2. MANTENERSE VOLANDO MIENTRAS MANTENGA PRESIONADO
        // ==========================================

        float tiempoVueloActual = 0f;

        // Se mantiene arriba mientras siga presionando la tecla
        // y no haya alcanzado el tiempo máximo permitido
        while (holdsAbilityKey && tiempoVueloActual < tiempoMaximoVuelo)
        {
            tiempoVueloActual += Time.fixedDeltaTime;

            Vector3 posicion = rb.position;
            // Mantiene la altura alcanzada
            posicion.y = rb.position.y;
            rb.MovePosition(posicion);

            yield return new WaitForFixedUpdate();
        }

        // ==========================================
        // 3. COMENZAR DESCENSO
        // ==========================================


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
        // ATERRIZAJE Y DETENCIÓN DEL AUDIO
        // ==========================================

        Vector3 velocidadFinal = rb.linearVelocity;

        velocidadFinal.y = 0f;
        rb.linearVelocity = velocidadFinal;

        // Detener sonido de aleteo al tocar el suelo
        if (emisor3D != null && emisor3D.isPlaying)
        {
            emisor3D.Stop();
        }

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