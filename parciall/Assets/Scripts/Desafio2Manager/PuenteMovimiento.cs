using UnityEngine;

public class PuenteMovimiento : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public float anguloMaximo = 15f;

    [Header("Desafío 2")]
    public Desafio2Manager manager;

    private Quaternion rotacionInicial;

    private void Start()
    {
        rotacionInicial = transform.rotation;
    }

    private void Update()
    {
        // Si los tres están en posición,
        // el puente deja de moverse.
        if (manager != null && manager.puentePreparado)
        {
            return;
        }

        // Movimiento normal del puente
        float angulo =
            Mathf.Sin(Time.time * velocidad) * anguloMaximo;

        transform.rotation =
            rotacionInicial *
            Quaternion.Euler(0f, 0f, angulo);
    }
}
