using UnityEngine;

public class PasosPersonaje : MonoBehaviour
{
    public AudioClip sonidoPaso;
    [Range(0f, 1f)] public float volumen = 0.5f;
    public float tiempoEntrePasos = 0.4f;

    private AudioSource emisor3D;
    private Vector3 ultimaPosicion;
    private float contadorTiempo;

    private void Start()
    {
        emisor3D = gameObject.AddComponent<AudioSource>();
        emisor3D.spatialBlend = 1f; // Sonido 3D
        emisor3D.minDistance = 1f;
        emisor3D.maxDistance = 15f;

        ultimaPosicion = transform.position;
    }

    private void Update()
    {
        // Medimos la velocidad real del personaje en el espacio
        float velocidad = (transform.position - ultimaPosicion).magnitude / Time.deltaTime;

        // Si se está moviendo a una velocidad considerable
        if (velocidad > 0.1f)
        {
            contadorTiempo += Time.deltaTime;

            if (contadorTiempo >= tiempoEntrePasos)
            {
                SonarPaso();
                contadorTiempo = 0f;
            }
        }
        else
        {
            // Si la velocidad cae a cero (se detiene), reseteamos el tiempo
            // y deteniene cualquier sonido activo al instante
            contadorTiempo = 0f;
            if (emisor3D.isPlaying)
            {
                emisor3D.Stop();
            }
        }

        ultimaPosicion = transform.position;
    }

    public void SonarPaso()
    {
        if (sonidoPaso != null && emisor3D != null)
        {
            emisor3D.pitch = Random.Range(0.9f, 1.1f);
            emisor3D.PlayOneShot(sonidoPaso, volumen);
        }
    }
}
