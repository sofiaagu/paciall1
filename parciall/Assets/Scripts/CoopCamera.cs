using UnityEngine;

public class CoopCamera : MonoBehaviour
{
    [Header("Jugadores")]
    public Transform[] players;

    [Header("Sección actual")]
    public Transform currentSection;

    [Header("Configuración de cámara")]
    public float distance = 8f;
    public float angleX = 45f;
    public float angleY = 45f;

    [Header("Movimiento entre secciones")]
    public float moveSpeed = 5f;

    [Header("Altura del objetivo")]
    public float targetHeight = 0f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    // Controla si se usa el objetivo acomodado manualmente
    private Transform manualTargetPoint;
    private bool useManualTarget = false;

    private void Start()
    {
        if (currentSection != null)
        {
            SetCameraImmediately(currentSection);
        }
    }

    private void LateUpdate()
    {
        if (useManualTarget && manualTargetPoint != null)
        {
            // Mueve suavemente la cámara hacia el punto que acomodaste con la mano
            transform.position = Vector3.Lerp(
                transform.position,
                manualTargetPoint.position,
                moveSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                manualTargetPoint.rotation,
                moveSpeed * Time.deltaTime
            );

            return;
        }

        if (currentSection == null)
            return;

        MoveCameraToSection();
    }

    private void MoveCameraToSection()
    {
        Quaternion rotation = Quaternion.Euler(angleX, angleY, 0f);
        Vector3 direction = rotation * Vector3.back;

        targetPosition = currentSection.position + direction * distance;
        targetPosition.y += targetHeight;

        targetRotation = rotation;

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

    public void ChangeSection(
        Transform newSection,
        float newDistance,
        float newAngleX,
        float newAngleY,
        float newTargetHeight
    )
    {
        if (newSection == null)
            return;

        useManualTarget = false;
        currentSection = newSection;

        distance = newDistance;
        angleX = newAngleX;
        angleY = newAngleY;
        targetHeight = newTargetHeight;
    }

    // ==========================================================
    // CAMBIAR A UN PUNTO ACOMODADO A MANO
    // ==========================================================
    public void ChangeToManualTarget(Transform manualTarget)
    {
        if (manualTarget == null) return;

        manualTargetPoint = manualTarget;
        useManualTarget = true;
    }

    private void SetCameraImmediately(Transform section)
    {
        Quaternion rotation = Quaternion.Euler(angleX, angleY, 0f);
        Vector3 direction = rotation * Vector3.back;

        transform.position = section.position + direction * distance;
        transform.position += Vector3.up * targetHeight;

        transform.rotation = rotation;
    }
}