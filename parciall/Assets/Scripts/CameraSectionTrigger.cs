using UnityEngine;

public class CameraSectionTrigger : MonoBehaviour
{
    [Header("Cámara Principal")]
    public CoopCamera coopCamera;

    [Header("Siguiente Sección")]
    public Transform nextSection;

    [Header("Configuración de Zoom para la Sección")]
    [Tooltip("Distancia de la cámara cuando los 4 jugadores están juntos en esta sección")]
    public float minDistance = 8f;

    [Tooltip("Distancia máxima de la cámara cuando los jugadores se alejan en esta sección")]
    public float maxDistance = 18f;

    [Header("Configuración de Ángulo y Altura")]
    public float angleX = 45f;
    public float angleY = 45f;
    public float targetHeight = 0f;

    [Header("Jugadores (Mono, Paloma, Serpiente, Venado)")]
    public Transform[] players;

    private bool[] playersInside;

    private bool sectionActivated;

    private void Start()
    {
        if (players != null && players.Length > 0)
        {
            playersInside = new bool[players.Length];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (players == null) return;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && other.transform == players[i])
            {
                playersInside[i] = true;

                CheckAllPlayersInside();

                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (players == null) return;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && other.transform == players[i])
            {
                playersInside[i] = false;

                return;
            }
        }
    }

    private void CheckAllPlayersInside()
    {
        if (sectionActivated || playersInside == null)
            return;

        for (int i = 0; i < playersInside.Length; i++)
        {
            // Si falta algún jugador por entrar al Trigger, no se cambia de sección
            if (!playersInside[i])
                return;
        }

        sectionActivated = true;

        // Llamada actualizada a CoopCamera enviando la distancia mínima y máxima de zoom
        if (coopCamera != null)
        {
            coopCamera.ChangeSection(
                nextSection,
                minDistance,
                maxDistance,
                angleX,
                angleY,
                targetHeight
            );
        }
    }
}