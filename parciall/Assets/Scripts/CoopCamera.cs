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

    private void Start()
    {
        if (currentSection != null)
        {
            SetCameraImmediately(currentSection);
        }
    }

    private void LateUpdate()
    {
        if (currentSection == null)
            return;

        MoveCameraToSection();
    }

    // ==========================================================
    // MOVER CÁMARA HACIA LA SECCIÓN
    // ==========================================================

    private void MoveCameraToSection()
    {
        Quaternion rotation =
            Quaternion.Euler(
                angleX,
                angleY,
                0f
            );

        Vector3 direction =
            rotation * Vector3.back;

        targetPosition =
            currentSection.position +
            direction * distance;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        transform.rotation = rotation;
    }

    // ==========================================================
    // CAMBIAR DE SECCIÓN
    // ==========================================================

    public void ChangeSection(Transform newSection)
    {
        if (newSection == null)
            return;

        currentSection = newSection;
    }

    // ==========================================================
    // COLOCAR CÁMARA DIRECTAMENTE
    // ==========================================================

    private void SetCameraImmediately(Transform section)
    {
        Quaternion rotation =
            Quaternion.Euler(
                angleX,
                angleY,
                0f
            );

        Vector3 direction =
            rotation * Vector3.back;

        transform.position =
            section.position +
            direction * distance;

        transform.rotation = rotation;
    }
}