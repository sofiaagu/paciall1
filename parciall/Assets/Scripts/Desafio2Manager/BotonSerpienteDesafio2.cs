using UnityEngine;

public class BotonSerpienteDesafio2 : MonoBehaviour
{
    public Desafio2Manager manager;

    private bool activado = false;

    public AudioSource sonidoActivacion;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (other.CompareTag("Serpiente"))
        {
            activado = true;

            manager.EstabilizarPuente();

            Debug.Log("La serpiente presionó el botón");

            if (sonidoActivacion != null)
                sonidoActivacion.Play();
        }
    }
}