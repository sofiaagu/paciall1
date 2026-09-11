using UnityEngine;
using UnityEngine.InputSystem;

public class VenadoEmpujar : MonoBehaviour
{
    [Header("Control")]
    public string controlScheme = "Gamepad";

    [Header("Configuración del empuje")]
    public float distanciaEmpuje = 1.5f;
    public float fuerzaEmpuje = 8f;

    [Header("Detección de caja")]
    public LayerMask cajaLayer;

    private NIS inputActions;

    private bool empujando = false;

    private void Awake()
    {
        inputActions = new NIS();

        // Utiliza únicamente el control asignado al venado
        inputActions.bindingMask =
            InputBinding.MaskByGroup(controlScheme);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        // Cuando PRESIONA el botón
        inputActions.Player.Ability.started += OnAbilityStarted;

        // Cuando SUELTA el botón
        inputActions.Player.Ability.canceled += OnAbilityCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Ability.started -= OnAbilityStarted;
        inputActions.Player.Ability.canceled -= OnAbilityCanceled;

        inputActions.Player.Disable();
    }

    private void OnAbilityStarted(InputAction.CallbackContext context)
    {
        empujando = true;

        Debug.Log("🦌 Venado comenzó a empujar");
    }

    private void OnAbilityCanceled(InputAction.CallbackContext context)
    {
        empujando = false;

        Debug.Log("🦌 Venado dejó de empujar");
    }

    private void FixedUpdate()
    {
        if (!empujando)
            return;

        EmpujarCaja();
    }

    private void EmpujarCaja()
    {
        // Punto desde donde detectamos la caja
        Vector3 origen =
            transform.position + Vector3.up * 0.5f;

        // Busca una caja delante del venado
        if (Physics.Raycast(
            origen,
            transform.forward,
            out RaycastHit hit,
            distanciaEmpuje,
            cajaLayer))
        {
            Rigidbody cajaRb =
                hit.collider.GetComponent<Rigidbody>();

            if (cajaRb != null)
            {
                // Dirección hacia donde mira el venado
                Vector3 direccion =
                    transform.forward;

                // Empuje continuo
                cajaRb.AddForce(
                    direccion * fuerzaEmpuje,
                    ForceMode.Force
                );
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origen =
            transform.position + Vector3.up * 0.5f;

        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            origen,
            transform.forward * distanciaEmpuje
        );
    }
}