using UnityEngine;

public class SonidoRuna : MonoBehaviour
{
    public AudioClip sonidoMagico;
    [Range(0f, 1f)] public float volumen = 0.7f;
    public float distanciaMinima = 1f;   // A esta distancia se escucha al 100%
    public float distanciaMaxima = 8f;   // A partir de aquí ya no se escucha

    private AudioSource emisor3D;

    private void Start()
    {
        emisor3D = gameObject.AddComponent<AudioSource>();
        emisor3D.clip = sonidoMagico;
        emisor3D.loop = true;          // Suena continuamente
        emisor3D.spatialBlend = 1f;    // 100% Sonido 3D
        emisor3D.minDistance = distanciaMinima;
        emisor3D.maxDistance = distanciaMaxima;
        emisor3D.volume = volumen;
        emisor3D.rolloffMode = AudioRolloffMode.Logarithmic; // Caída natural de sonido

        if (sonidoMagico != null)
        {
            emisor3D.Play();
        }
    }
}

