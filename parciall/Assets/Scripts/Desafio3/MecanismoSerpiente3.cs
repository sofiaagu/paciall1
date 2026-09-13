using UnityEngine;

public class MecanismoSerpiente3 : MonoBehaviour
{
    [Header("Plataforma que controla")]
    public PlataformaMovimiento3 plataforma;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (other.CompareTag("Serpiente"))
        {
            activado = true;

            if (plataforma != null)
            {
                plataforma.DetenerPlataforma();
            }

            Debug.Log("🐍 La Serpiente activó el mecanismo");
        }
    }
}