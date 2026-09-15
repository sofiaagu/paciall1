using UnityEngine;

public class MecanismoVenado3 : MonoBehaviour
{
    [Header("Plataforma que controla")]
    public PlataformaMovimiento3 plataforma;

    private bool activado = false;

    public AudioSource sonidoActivacion;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        // Busca el script del Venado en el objeto o en sus padres
        VenadoEmpujar venado = other.GetComponentInParent<VenadoEmpujar>();

        if (venado != null)
        {
            activado = true;

            if (plataforma != null)
            {
                plataforma.DetenerPlataforma();
            }

            Debug.Log("El Venado activó el mecanismo");
            if (sonidoActivacion != null)
                sonidoActivacion.Play();
        }
    }
}