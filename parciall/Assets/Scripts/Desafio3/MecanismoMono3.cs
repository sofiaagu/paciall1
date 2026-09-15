using UnityEngine;

public class MecanismoMono3 : MonoBehaviour
{
    [Header("Plataforma que controla")]
    public PlataformaMovimiento3 plataforma;
    public AudioSource sonidoActivacion;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (other.CompareTag("Mono"))
        {
            activado = true;

            if (plataforma != null)
            {
                plataforma.DetenerPlataforma();
            }

            Debug.Log("El Mono activó el mecanismo");

            if (sonidoActivacion != null)
                sonidoActivacion.Play();
        }
    }
}