using System.Collections.Generic;
using UnityEngine;

public class CoopCamera : MonoBehaviour
{
    [Header("Jugadores a Seguir")]
    [Tooltip("Arrastra aquí a los 4 jugadores (Mono, Paloma, Serpiente, Venado)")]
    public Transform[] players;

    [Header("Sección actual")]
    public Transform currentSection;

    [Header("Configuración de Ángulos")]
    public float angleX = 45f;
    public float angleY = 45f;
    public float targetHeight = 0f;

    [Header("Configuración de Zoom Dinámico")]
    [Tooltip("Distancia mínima de la cámara cuando los jugadores están juntos")]
    public float minDistance = 8f;

    [Tooltip("Distancia máxima de la cámara cuando los jugadores se alejan")]
    public float maxDistance = 20f;

    [Tooltip("Separación entre jugadores para alcanzar la distancia máxima de zoom")]
    public float maxPlayerSpread = 15f;

    [Header("Movimiento y Suavizado")]
    public float moveSpeed = 5f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Start()
    {
        if (currentSection != null)
        {
            SetCameraImmediately(currentSection);
        }
    }

    private void LateUpdate()
    {
        if (players == null || players.Length == 0)
            return;

        MoveCameraWithDynamicZoom();
    }

    // ==========================================================
    // SEGUIMIENTO DE JUGADORES + ZOOM DINÁMICO
    // ==========================================================

    private void MoveCameraWithDynamicZoom()
    {
        // 1. Obtener los límites del grupo de jugadores
        Bounds bounds = GetPlayersBounds();

        // Si no hay jugadores válidos en la escena, no hacemos nada
        if (bounds.size == Vector3.zero && bounds.center == Vector3.zero) return;

        // 2. Calcular el punto medio (centro del grupo)
        Vector3 centerPoint = bounds.center;

        // 3. Calcular la distancia máxima entre los personajes más alejados
        float greatestDistance = Mathf.Max(bounds.size.x, bounds.size.z);
        float zoomFactor = Mathf.Clamp01(greatestDistance / maxPlayerSpread);

        // 4. Determinar la distancia dinámica (zoom)
        float currentDistance = Mathf.Lerp(minDistance, maxDistance, zoomFactor);

        // 5. Calcular la rotación basándonos en angleX y angleY
        Quaternion rotation = Quaternion.Euler(angleX, angleY, 0f);
        Vector3 direction = rotation * Vector3.back;

        // 6. La posición objetivo se basa en el CENTRO DEL GRUPO + el offset de distancia y altura
        targetPosition = centerPoint + (direction * currentDistance);
        targetPosition.y += targetHeight;

        targetRotation = rotation;

        // 7. Aplicar el movimiento y rotación suavizados
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            moveSpeed * Time.deltaTime
        );
    }

    // ==========================================================
    // CÁLCULO DE LÍMITES Y CENTRO
    // ==========================================================

    private Bounds GetPlayersBounds()
    {
        Bounds bounds = new Bounds();
        bool firstPlayer = false;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && players[i].gameObject.activeInHierarchy)
            {
                if (!firstPlayer)
                {
                    bounds = new Bounds(players[i].position, Vector3.zero);
                    firstPlayer = true;
                }
                else
                {
                    bounds.Encapsulate(players[i].position);
                }
            }
        }

        return bounds;
    }

    // ==========================================================
    // CAMBIAR DE SECCIÓN (Soporta disparadores/triggers)
    // ==========================================================

    public void ChangeSection(
        Transform newSection,
        float newMinDistance,
        float newMaxDistance,
        float newAngleX,
        float newAngleY,
        float newTargetHeight
    )
    {
        if (newSection == null)
            return;

        currentSection = newSection;
        minDistance = newMinDistance;
        maxDistance = newMaxDistance;
        angleX = newAngleX;
        angleY = newAngleY;
        targetHeight = newTargetHeight;
    }

    // ==========================================================
    // COLOCAR CÁMARA DIRECTAMENTE AL INICIAR
    // ==========================================================

    private void SetCameraImmediately(Transform section)
    {
        Bounds bounds = GetPlayersBounds();
        Vector3 targetCenter = (bounds.size != Vector3.zero) ? bounds.center : section.position;

        Quaternion rotation = Quaternion.Euler(angleX, angleY, 0f);
        Vector3 direction = rotation * Vector3.back;

        transform.position = targetCenter + (direction * minDistance);
        transform.position += Vector3.up * targetHeight;
        transform.rotation = rotation;
    }
}