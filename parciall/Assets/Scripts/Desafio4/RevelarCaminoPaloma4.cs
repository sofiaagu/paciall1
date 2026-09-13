using UnityEngine;

public class RevelarCaminoPaloma4 : MonoBehaviour
{
    [Header("Piedras que revela la Paloma")]
    public GameObject[] piedras;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (!other.CompareTag("Paloma"))
            return;

        activado = true;

        foreach (GameObject piedra in piedras)
        {
            if (piedra != null)
            {
                piedra.SetActive(true);
            }
        }

        Debug.Log("🕊️ La Paloma reveló la segunda parte del camino");
    }
}