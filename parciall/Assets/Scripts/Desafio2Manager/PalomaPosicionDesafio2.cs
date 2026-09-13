using UnityEngine;

public class PalomaPosicionDesafio2 : MonoBehaviour
{
    public Desafio2Manager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Paloma"))
        {
            manager.palomaEnPosicion = true;

            Debug.Log("🕊️ La paloma está en su posición");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Paloma"))
        {
            manager.palomaEnPosicion = false;

            Debug.Log("🕊️ La paloma salió de su posición");
        }
    }
}