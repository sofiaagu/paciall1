using UnityEngine;

public class CajaObjetivo3 : MonoBehaviour
{
    [Header("Plataforma que se detiene")]
    public PlataformaMovimiento3 plataforma;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (other.CompareTag("Caja"))
        {
            activado = true;

            if (plataforma != null)
            {
                plataforma.DetenerPlataforma();
            }

            Debug.Log("📦 La caja llegó a su posición");
            Debug.Log("🦌 El Venado completó su parte");
        }
    }
}