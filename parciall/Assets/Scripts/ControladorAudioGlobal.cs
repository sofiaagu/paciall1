using UnityEngine;

public class ControladorAudioGlobal : MonoBehaviour
{
    public static ControladorAudioGlobal Instance;

    [Header("Referencias")]
    public AudioSource audioSource;

    [Header("Canciones")]
    public AudioClip musicaNormal;  // Arrastra aquí la música del menú
    public AudioClip musicaTensa;  // Arrastra aquí la música del panel 3 y 4

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Al iniciar el juego / menú, suena la música normal
        ReproducerMusicaNormal();
    }

    public void ReproducerMusicaNormal()
    {
        if (audioSource != null && musicaNormal != null)
        {
            if (audioSource.clip != musicaNormal || !audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = musicaNormal;
                audioSource.Play();
            }
        }
    }

    public void ReproducirMusicaTensa()
    {
        if (audioSource != null && musicaTensa != null)
        {
            if (audioSource.clip != musicaTensa || !audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = musicaTensa;
                audioSource.Play();
            }
        }
    }
}