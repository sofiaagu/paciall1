using UnityEngine;
using UnityEngine.InputSystem;

public class TaipanAbility : MonoBehaviour
{
    private NIS inputActions;

    [Header("Control Scheme")]
    public string controlScheme = "Numpad";
    
    [Header("Ajustes del Capsule Collider")]
    public float radioReptar = 0.2f;
    public float alturaReptar = 0.4f;
    public Vector3 centroReptar = new Vector3(0f, 0.2f, 0f);

    [Header("Animación")]
    public Animator animator;

    private CapsuleCollider capsuleCollider;
    private Vector3 centroOriginal;
    private float radioOriginal;
    private float alturaOriginal;

    // Estado para controlar el modo Toggle
    private bool estaReptando = false;

    private void Awake()
    {
        inputActions = new NIS();
        inputActions.bindingMask = InputBinding.MaskByGroup(controlScheme);

        capsuleCollider = GetComponent<CapsuleCollider>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (capsuleCollider != null)
        {
            centroOriginal = capsuleCollider.center;
            radioOriginal = capsuleCollider.radius;
            alturaOriginal = capsuleCollider.height;
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        // Escuchamos solo cuando se presiona la tecla
        inputActions.Player.Ability.performed += OnAbilityPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Ability.performed -= OnAbilityPerformed;
        inputActions.Player.Disable();
    }

    private void OnAbilityPerformed(InputAction.CallbackContext context)
    {
        // Alternamos el estado cada vez que se presiona la tecla
        estaReptando = !estaReptando;

        if (estaReptando)
        {
            EmpezarAReptar();
        }
        else
        {
            DetenerReptar();
        }
    }

    private void EmpezarAReptar()
    {
        if (capsuleCollider != null)
        {
            capsuleCollider.center = centroReptar;
            capsuleCollider.radius = radioReptar;
            capsuleCollider.height = alturaReptar;
        }

        if (animator != null)
        {
            animator.SetBool("IsCrawling", true);
        }
    }

    private void DetenerReptar()
    {
        if (capsuleCollider != null)
        {
            capsuleCollider.center = centroOriginal;
            capsuleCollider.radius = radioOriginal;
            capsuleCollider.height = alturaOriginal;
        }

        if (animator != null)
        {
            animator.SetBool("IsCrawling", false);
        }
    }
}