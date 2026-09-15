using UnityEngine;

public class SonidoRio : MonoBehaviour
{
    [Header("Configuración del Río")]
    public AudioClip sonidoAgua;
    [Range(0f, 1f)] public float volumen = 0.8f;

    [Header("Ajustes 3D Espaciales")]
    public float distanciaMinima = 2f;  // Se escucha al 100% al estar muy cerca
    public float distanciaMaxima = 12f; // Se deja de escuchar al alejarse

    private AudioSource emisorRio;

    private void Start()
    {
        emisorRio = gameObject.AddComponent<AudioSource>();
        emisorRio.clip = sonidoAgua;
        emisorRio.loop = true;
        emisorRio.spatialBlend = 1f; // 100% sonido espacial 3D
        emisorRio.minDistance = distanciaMinima;
        emisorRio.maxDistance = distanciaMaxima;
        emisorRio.volume = volumen;
        emisorRio.rolloffMode = AudioRolloffMode.Logarithmic; // Atenuación natural

        if (sonidoAgua != null)
        {
            emisorRio.Play();
        }
    }
}