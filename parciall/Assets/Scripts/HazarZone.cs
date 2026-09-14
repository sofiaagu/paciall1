using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HazarZone : MonoBehaviour
{
    [Header("Configuración del Peligro")]
    [Tooltip("Tiempo de espera antes de reiniciar la escena (para reproducir animación o sonido)")]
    public float delayReinicio = 0.5f;

    [Tooltip("Etiquetas permitidas para activar el peligro")]
    public string[] tagsJugadores = { "Mono", "Paloma", "Serpiente", "Venado", "Player", "Caja" };

    private bool reiniciando = false;

    private void OnTriggerEnter(Collider other)
    {
        if (reiniciando)
            return;

        if (EsObjetoValido(other.gameObject))
        {
            reiniciando = true;
            Debug.Log($"⚠️ {other.gameObject.name} cayó en un área peligrosa ({gameObject.name}). Reiniciando nivel...");
            StartCoroutine(ReiniciarNivelRutina());
        }
    }

    private bool EsObjetoValido(GameObject obj)
    {
        foreach (string t in tagsJugadores)
        {
            if (obj.CompareTag(t))
                return true;
        }

        // Comprobación adicional por si el collider está en un hijo del personaje
        foreach (string t in tagsJugadores)
        {
            if (obj.GetComponentInParent<Transform>()?.CompareTag(t) == true)
                return true;
        }

        return false;
    }

    private IEnumerator ReiniciarNivelRutina()
    {
        yield return new WaitForSeconds(delayReinicio);

        // Carga la escena activa actual
        int escenaActualIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActualIndex);
    }
}