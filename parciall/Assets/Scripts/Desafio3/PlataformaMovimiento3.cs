using UnityEngine;

public class PlataformaMovimiento3 : MonoBehaviour
{
    [Header("Movimiento")]
    public float distancia = 3f;
    public float velocidad = 2f;

    [Header("Dirección")]
    public Vector3 direccion = Vector3.right;

    [Header("Posición final")]
    public Transform posicionCorrecta;

    private Vector3 posicionInicial;

    private bool detenida = false;

    private void Start()
    {
        posicionInicial = transform.position;
    }

    private void Update()
    {
        // Si está detenida, no sigue moviéndose
        if (detenida)
            return;

        float movimiento =
            Mathf.Sin(Time.time * velocidad) * distancia;

        transform.position =
            posicionInicial +
            direccion.normalized * movimiento;
    }

    // ==========================================
    // DETENER PLATAFORMA
    // ==========================================

    public void DetenerPlataforma()
    {
        detenida = true;

        // Colocar la plataforma exactamente
        // en la posición correcta
        if (posicionCorrecta != null)
        {
            transform.position =
                posicionCorrecta.position;

            transform.rotation =
                posicionCorrecta.rotation;
        }

        Debug.Log(
            "🟫 Plataforma detenida en su posición correcta"
        );
    }
}