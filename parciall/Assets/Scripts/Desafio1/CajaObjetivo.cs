using UnityEngine;

public class CajaObjetivo : MonoBehaviour
{
    public Desafio1Manager manager;
    public AudioSource sonidoActivacion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Caja"))
        {
            manager.cajaEnPosicion = true;

            Debug.Log("La caja está en la posición correcta");

            if (sonidoActivacion != null)
                sonidoActivacion.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Caja"))
        {
            manager.cajaEnPosicion = false;

            Debug.Log("La caja salió de la posición");
        }
    }
}