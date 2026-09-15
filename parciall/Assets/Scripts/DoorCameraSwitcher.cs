using System.Collections.Generic;
using UnityEngine;

public class DoorCameraSwitcher : MonoBehaviour
{
    [Header("Cámara Principal")]
    public Transform mainCamera;

    [Header("Punto de destino para la cámara")]
    public Transform targetTransform;

    [Header("Velocidad de transición")]
    public float transitionSpeed = 3f;

    [Header("Jugadores requeridos (4 animales)")]
    public Transform[] players;

    [Header("Configuración del Río / Respawn")]
    public HazarZone hazarZoneScript; // Arrastra la HazarZone del río
    public Transform spawnPointParte2; // Arrastra el punto de Spawn de la Parte 2

    private HashSet<Transform> playersWhoPassed = new HashSet<Transform>();
    private bool isTransitioning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning) return;

        foreach (Transform player in players)
        {
            if (player != null && (other.transform == player || other.transform.IsChildOf(player)))
            {
                if (!playersWhoPassed.Contains(player))
                {
                    playersWhoPassed.Add(player);
                    Debug.Log($"¡{player.name} cruzó la puerta! ({playersWhoPassed.Count}/{players.Length})");
                }

                CheckAllPlayers();
                break;
            }
        }
    }

    private void CheckAllPlayers()
    {
        if (playersWhoPassed.Count >= players.Length)
        {
            // Apaga el CoopCamera si existe
            CoopCamera coopScript = mainCamera.GetComponent<CoopCamera>();
            if (coopScript != null)
            {
                coopScript.enabled = false;
            }

            // Cambiar el punto de respawn del agua a la Parte 2
            if (hazarZoneScript != null && spawnPointParte2 != null)
            {
                hazarZoneScript.currentRespawnPoint = spawnPointParte2;
                Debug.Log("<color=yellow>¡Punto de Respawn del agua actualizado a Parte 2!</color>");
            }

            isTransitioning = true;
            Debug.Log("<color=green>¡Los 4 cruzaron! Moviendo cámara...</color>");
        }
    }

    private void Update()
    {
        if (isTransitioning && mainCamera != null && targetTransform != null)
        {
            mainCamera.position = Vector3.Lerp(
                mainCamera.position, 
                targetTransform.position, 
                Time.deltaTime * transitionSpeed
            );

            mainCamera.rotation = Quaternion.Slerp(
                mainCamera.rotation, 
                targetTransform.rotation, 
                Time.deltaTime * transitionSpeed
            );

            if (Vector3.Distance(mainCamera.position, targetTransform.position) < 0.01f)
            {
                mainCamera.position = targetTransform.position;
                mainCamera.rotation = targetTransform.rotation;
                isTransitioning = false;
            }
        }
    }
}