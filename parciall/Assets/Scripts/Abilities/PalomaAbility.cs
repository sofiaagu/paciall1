using UnityEngine;
using UnityEngine.InputSystem;

public class PigeonAbility : MonoBehaviour
{
    private NIS inputActions;
    private MovePlayer movePlayer;

    [Header("Configuración de vuelo")]
    public float alturaVuelo = 3f;
    public float duracionVuelo = 2f;

    [Header("Animación")]
    public Animator animator;

    private bool isFlying;

    private void Awake()
    {
        inputActions = new NIS();

        inputActions.bindingMask =
            InputBinding.MaskByGroup("Keyboard_arrows");

        movePlayer = GetComponent<MovePlayer>();

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
        // Ya está volando
        if (isFlying)
            return;

        // Está saltando o en el aire
        if (!movePlayer.IsGrounded)
            return;

        StartCoroutine(Fly());
    }

    private System.Collections.IEnumerator Fly()
    {
        isFlying = true;

        float alturaInicial = transform.position.y;

        // Activar animación de vuelo
        if (animator != null)
        {
            animator.SetBool("Flying", true);
        }

        float tiempo = 0f;

        while (tiempo < duracionVuelo)
        {
            tiempo += Time.deltaTime;

            float progreso = tiempo / duracionVuelo;

            float altura =
                Mathf.Sin(progreso * Mathf.PI) * alturaVuelo;

            Vector3 posicion = transform.position;

            posicion.y = alturaInicial + altura;

            transform.position = posicion;

            yield return null;
        }

        Vector3 posicionFinal = transform.position;
        posicionFinal.y = alturaInicial;

        transform.position = posicionFinal;

        // Termina la animación de vuelo
        if (animator != null)
        {
            animator.SetBool("Flying", false);
        }

        isFlying = false;
    }
}