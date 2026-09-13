using UnityEngine;

public class MonoPosicionDesafio2 : MonoBehaviour
{
    public Desafio2Manager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mono"))
        {
            manager.monoEnPosicion = true;

            Debug.Log("🐒 El mono está en su posición");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mono"))
        {
            manager.monoEnPosicion = false;

            Debug.Log("🐒 El mono salió de su posición");
        }
    }
}