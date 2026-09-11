using UnityEngine;

public class CajaObjetivo2 : MonoBehaviour
{
    public Desafio2Manager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Caja"))
        {
            manager.cajaEnPosicion = true;

            Debug.Log("📦 La caja está en la posición del desafío 2");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Caja"))
        {
            manager.cajaEnPosicion = false;

            Debug.Log("📦 La caja salió de la posición");
        }
    }
}
