using UnityEngine;

public class ArrastreObjeto : MonoBehaviour
{
    public AudioClip sonidoArrastre;
    [Range(0f, 1f)] public float volumenMaximo = 0.8f;

    private AudioSource emisor3D;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Crearmos el emisor 3D en la piedra
        emisor3D = gameObject.AddComponent<AudioSource>();
        emisor3D.clip = sonidoArrastre;
        emisor3D.loop = true;          // Sonido continuo
        emisor3D.spatialBlend = 1f;    // 100% 3D
        emisor3D.minDistance = 1f;
        emisor3D.maxDistance = 15f;
    }

    private void Update()
    {
        if (rb == null || sonidoArrastre == null) return;

        // Medimos si la piedra se está moviendo físicamente
        float velocidad = rb.linearVelocity.magnitude;

        if (velocidad > 0.1f)
        {
            if (!emisor3D.isPlaying)
            {
                emisor3D.Play();
            }

            emisor3D.volume = Mathf.Clamp01(velocidad / 3f) * volumenMaximo;
        }
        else
        {
            if (emisor3D.isPlaying)
            {
                emisor3D.Stop();
            }
        }
    }
}
