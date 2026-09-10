using UnityEngine;

public class CajaObjetivo : MonoBehaviour
{
    public Desafio1Manager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Caja"))
        {
            manager.cajaEnPosicion = true;

            Debug.Log("La caja está en la posición correcta");
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