using UnityEngine;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour
{
    [Header("Configuración de Respawn")]
    [Tooltip("Punto de reaparición para la Parte 4 (ej: SpawnPoint_Parte4)")]
    public Transform nuevoSpawnPoint;

    [Tooltip("Objeto que tiene el script HazarZone en la escena")]
    public HazarZone hazarZone;

    [Header("Configuración de Cámara")]
    [Tooltip("La Main Camera que tiene el script CoopCamera")]
    public CoopCamera coopCamera;

    [Tooltip("La sección destino de la cámara (ej: seccion2)")]
    public Transform nuevaSeccionCamara;

    [Header("Condición de Activación")]
    [Tooltip("Cantidad de personajes requeridos para mover la cámara")]
    public int totalPersonajesRequeridos = 4;

    [Header("Etiquetas Válidas")]
    public string[] tagsValidos = { "Mono", "Paloma", "Serpiente", "Venado", "Player" };

    // Guarda los personajes que ya cruzaron para no contar dos veces el mismo
    private HashSet<GameObject> personajesEnZona = new HashSet<GameObject>();
    private bool checkpointActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (checkpointActivado) return;

        GameObject raiz = ObtenerRaizValida(other.gameObject);

        if (raiz != null && !personajesEnZona.Contains(raiz))
        {
            personajesEnZona.Add(raiz);
            Debug.Log($"🐾 Personaje llegó al Checkpoint: {raiz.name} ({personajesEnZona.Count}/{totalPersonajesRequeridos})");

            // Cuando los 4 personajes están en la Parte 4
            if (personajesEnZona.Count >= totalPersonajesRequeridos)
            {
                ActivarTransicion();
            }
        }
    }

    private void ActivarTransicion()
    {
        checkpointActivado = true;

        // 1. Actualizar el SpawnPoint de la HazarZone (pinchos/agua)
        if (hazarZone != null && nuevoSpawnPoint != null)
        {
            hazarZone.currentRespawnPoint = nuevoSpawnPoint;
            Debug.Log($"🚩 Checkpoint activado! Respawn actualizado a: {nuevoSpawnPoint.name}");
        }

        // 2. Mover la CoopCamera a la nueva sección
        if (coopCamera != null && nuevaSeccionCamara != null)
        {
            coopCamera.currentSection = nuevaSeccionCamara;
            Debug.Log($"🎥 Los 4 llegaron. Cámara movida a: {nuevaSeccionCamara.name}");
        }
    }

    private GameObject ObtenerRaizValida(GameObject obj)
    {
        Transform t = obj.transform;
        while (t != null)
        {
            foreach (string tag in tagsValidos)
            {
                if (t.CompareTag(tag))
                    return t.gameObject;
            }
            t = t.parent;
        }
        return null;
    }
}