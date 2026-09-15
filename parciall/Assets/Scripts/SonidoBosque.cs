using UnityEngine;

public class SonidoBosque : MonoBehaviour
{
    public AudioClip musicaAmbiente;

    private void Start()
    {
        AudioSource emisor = gameObject.AddComponent<AudioSource>();
        emisor.clip = musicaAmbiente;
        emisor.loop = true;
        emisor.spatialBlend = 0f; // Sonido 2D
        emisor.volume = 0.030f;
        emisor.Play();
    }
}
