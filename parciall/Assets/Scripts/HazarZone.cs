using UnityEngine;
using System.Collections;

public class HazarZone : MonoBehaviour
{
    [Header("Configuración del Peligro")]
    [Tooltip("Tiempo de espera antes de teletransportar (para animaciones o sonido)")]
    public float delayReinicio = 0.2f;

    [Tooltip("Etiquetas permitidas para activar el peligro")]
    public string[] tagsJugadores = { "Mono", "Paloma", "Serpiente", "Venado", "Player", "Caja" };

    [Header("Punto de Reaparición Actual")]
    [Tooltip("Arrastra aquí el SpawnPoint activo.")]
    public Transform currentRespawnPoint;

    [Header("Audio de Caída / Muerte")]
    public AudioClip sonidoCaida;
    [Range(0f, 1f)] public float volumenCaida = 0.8f;

    private bool procesandoCaida = false;

    private void OnTriggerEnter(Collider other)
    {
        if (procesandoCaida)
            return;

        if (EsObjetoValido(other.gameObject))
        {
            procesandoCaida = true;

            // Reproduce el sonido 3D en el punto exacto de la caída antes de respawnear
            if (sonidoCaida != null)
            {
                AudioSource.PlayClipAtPoint(sonidoCaida, other.transform.position, volumenCaida);
            }

            Debug.Log($"⚠️ {other.gameObject.name} cayó en un área peligrosa ({gameObject.name}). Teletransportando...");
            StartCoroutine(TeletransportarRutina(other.gameObject));
        }
    }

    private bool EsObjetoValido(GameObject obj)
    {
        // Verificar si el objeto directo o alguno de sus padres tiene una etiqueta válida
        Transform t = obj.transform;
        while (t != null)
        {
            foreach (string tag in tagsJugadores)
            {
                if (t.CompareTag(tag))
                    return true;
            }
            t = t.parent;
        }

        return false;
    }

    private IEnumerator TeletransportarRutina(GameObject obj)
    {
        yield return new WaitForSeconds(delayReinicio);

        if (currentRespawnPoint != null)
        {
            // 1. Buscamos el Transform raíz del personaje que tiene la etiqueta
            Transform targetTransform = ObtenerTransformPersonaje(obj);

            // 2. Buscamos sus componentes de física/movimiento
            CharacterController cc = targetTransform.GetComponent<CharacterController>();
            Rigidbody rb = targetTransform.GetComponent<Rigidbody>();

            // 3. Desactivamos CharacterController temporalmente si existe
            if (cc != null)
            {
                cc.enabled = false;
            }

            // 4. Limpiamos la velocidad del Rigidbody
            if (rb != null)
            {
                if (!rb.isKinematic)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }

            // 5. Mover al personaje directamente al SpawnPoint
            targetTransform.position = currentRespawnPoint.position;
            targetTransform.rotation = currentRespawnPoint.rotation;

            // 6. Reactivar CharacterController si existía
            if (cc != null)
            {
                cc.enabled = true;
            }

            Debug.Log($"✅ Reaparecido {targetTransform.name} en {currentRespawnPoint.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ No asignaste 'currentRespawnPoint' en HazarZone.");
        }

        procesandoCaida = false;
    }

    private Transform ObtenerTransformPersonaje(GameObject obj)
    {
        // Revisa la jerarquía hacia arriba para encontrar exactamente el objeto raíz con la etiqueta
        Transform current = obj.transform;
        while (current != null)
        {
            foreach (string t in tagsJugadores)
            {
                if (current.CompareTag(t))
                    return current;
            }
            current = current.parent;
        }

        return obj.transform;
    }
}