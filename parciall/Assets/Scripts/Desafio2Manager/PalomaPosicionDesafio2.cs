using UnityEngine;

public class PalomaPosicionDesafio2 : MonoBehaviour
{
    public Desafio2Manager manager;
    public AudioSource sonidoActivacion;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Paloma"))
        {
            manager.palomaEnPosicion = true;

            Debug.Log("La paloma está en su posición");
            if (sonidoActivacion != null)
                sonidoActivacion.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Paloma"))
        {
            manager.palomaEnPosicion = false;

            Debug.Log("La paloma salió de su posición");
        }
    }
}