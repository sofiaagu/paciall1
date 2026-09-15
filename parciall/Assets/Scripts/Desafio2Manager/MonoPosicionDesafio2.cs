using UnityEngine;

public class MonoPosicionDesafio2 : MonoBehaviour
{
    public Desafio2Manager manager;

    public AudioSource sonidoActivacion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mono"))
        {
            manager.monoEnPosicion = true;

            Debug.Log("El mono está en su posición");

            if (sonidoActivacion != null)
                sonidoActivacion.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mono"))
        {
            manager.monoEnPosicion = false;

            Debug.Log("El mono salió de su posición");
        }
    }
}