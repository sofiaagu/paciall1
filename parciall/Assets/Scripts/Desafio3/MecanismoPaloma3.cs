using UnityEngine;

public class MecanismoPaloma3 : MonoBehaviour
{
    [Header("Plataforma que controla")]
    public PlataformaMovimiento3 plataforma;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (other.CompareTag("Paloma"))
        {
            activado = true;

            if (plataforma != null)
            {
                plataforma.DetenerPlataforma();
            }

            Debug.Log("🕊️ La Paloma activó el mecanismo");
        }
    }
}