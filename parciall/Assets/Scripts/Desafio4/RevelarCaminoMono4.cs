using UnityEngine;

public class RevelarCaminoMono4 : MonoBehaviour
{
    [Header("Piedras que revela el Mono")]
    public GameObject[] piedras;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (!other.CompareTag("Mono"))
            return;

        activado = true;

        // Mostrar las piedras
        foreach (GameObject piedra in piedras)
        {
            if (piedra != null)
            {
                piedra.SetActive(true);
            }
        }

        Debug.Log("🐒 El Mono reveló la primera parte del camino");
    }
}