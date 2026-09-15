using UnityEngine;
using System.Collections;

public class FogSmoothFade : MonoBehaviour
{
    /// <summary>
    /// Controla un fade-out suave de un sistema de partículas tipo niebla cuando el jugador entra al área.
    /// 
    /// FUNCIONAMIENTO:
    /// 
    /// - Detecta cuando un objeto con el tag definido (por defecto "Player") entra al trigger.
    /// - Al activarse, inicia una corrutina que realiza un desvanecimiento progresivo de la niebla.
    /// - El fade-out incluye:
    ///     1. Disminuir gradualmente la tasa de emisión (rateOverTime).
    ///     2. Reducir la opacidad (alpha) del color inicial de las partículas.
    ///     3. (Opcional) Reducir su tamaño si se habilita el código comentado.
    /// - El efecto dura el tiempo indicado en fadeDuration.
    /// - Cuando termina el fade, detiene completamente el ParticleSystem.
    /// 
    /// Este script permite eliminar de forma suave la niebla en una sala al entrar el jugador,
    /// dando un efecto visual más natural que simplemente apagar el objeto.
    /// </summary>
    public ParticleSystem fogSystem;
    public float fadeDuration = 2f;    // duración del fade-out
    public string playerTag = "Player";

    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;

    private void Start()
    {
        mainModule = fogSystem.main;
        emissionModule = fogSystem.emission;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            StartCoroutine(FadeOutFog());
        }
    }

    IEnumerator FadeOutFog()
    {
        float startRate = emissionModule.rateOverTime.constant;
        float startAlpha = mainModule.startColor.color.a;
        //float startSize = mainModule.startSize.constant;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;

            // 1. Reducir emisión
            emissionModule.rateOverTime = Mathf.Lerp(startRate, 0, progress);

            // 2. Reducir la opacidad (alpha)
            Color c = mainModule.startColor.color;
            c.a = Mathf.Lerp(startAlpha, 0, progress);
            mainModule.startColor = c;

            // 3. Reducir el tamaño
            //mainModule.startSize = Mathf.Lerp(startSize, 0.1f, progress);

            yield return null;
        }

        fogSystem.Stop();
    }
}