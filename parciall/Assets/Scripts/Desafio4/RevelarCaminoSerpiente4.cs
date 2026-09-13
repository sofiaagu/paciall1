using UnityEngine;

public class RevelarCaminoSerpiente4 : MonoBehaviour
{
    [Header("Piedras que revela la Serpiente")]
    public GameObject[] piedras;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (!other.CompareTag("Serpiente"))
            return;

        activado = true;

        foreach (GameObject piedra in piedras)
        {
            if (piedra != null)
            {
                piedra.SetActive(true);
            }
        }

        Debug.Log("🐍 La Serpiente reveló la última parte del camino");
    }
}