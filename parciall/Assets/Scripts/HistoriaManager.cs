using System.Collections;
using UnityEngine;
using TMPro;

public class HistoriaManager : MonoBehaviour
{
    [System.Serializable]
    public struct LineaDialogo
    {
        public GameObject globoDialogo; // El objeto Globo (ej. Globo_Milo)
        public TextMeshProUGUI textoTMP;  // El componente TextMeshPro dentro del globo
        [TextArea(2, 4)]
        public string textoCompleto;    // El texto que aparecerá
    }

    [Header("Configuración de Diálogos")]
    public LineaDialogo[] lineas;
    public float velocidadEscritura = 0.04f;

    [Header("Audio SFX")]
    public AudioSource audioSourceTuc; // Asigna el AudioSource con el sonido 'tuc'
    public AudioClip sonidoTuc;        // Tu clip de audio 'tuc' corto

    [Header("UI")]
    public GameObject botonSiguiente;

    private int indiceActual = 0;
    private bool estaEscribiendo = false;

    void Start()
    {
        // Ocultar todos los globos al iniciar
        foreach (var linea in lineas)
        {
            if (linea.globoDialogo != null)
                linea.globoDialogo.SetActive(false);
        }

        if (botonSiguiente != null)
            botonSiguiente.SetActive(false);

        // Iniciar el primer diálogo
        if (lineas.Length > 0)
        {
            StartCoroutine(EscribirLinea(0));
        }
    }

    public void AvanzarDialogo()
    {
        // Si aún se está escribiendo la frase actual, la completa de golpe
        if (estaEscribiendo)
        {
            StopAllCoroutines();
            lineas[indiceActual].textoTMP.text = lineas[indiceActual].textoCompleto;
            estaEscribiendo = false;
            return;
        }

        // Apagar el globo actual antes de pasar al siguiente
        if (lineas[indiceActual].globoDialogo != null)
        {
            lineas[indiceActual].globoDialogo.SetActive(false);
        }

        indiceActual++;

        // Si quedan más líneas, escribir la siguiente
        if (indiceActual < lineas.Length)
        {
            StartCoroutine(EscribirLinea(indiceActual));
        }
        else
        {
            // Fin de la historia: Mostrar botón Siguiente para cambiar de escena/pantalla
            if (botonSiguiente != null)
                botonSiguiente.SetActive(true);
        }
    }

    IEnumerator EscribirLinea(int indice)
    {
        estaEscribiendo = true;
        LineaDialogo linea = lineas[indice];

        if (linea.globoDialogo != null)
            linea.globoDialogo.SetActive(true);

        linea.textoTMP.text = "";

        foreach (char letra in linea.textoCompleto.ToCharArray())
        {
            linea.textoTMP.text += letra;

            // Reproducir sonido "tuc" si no es un espacio
            if (letra != ' ' && audioSourceTuc != null && sonidoTuc != null)
            {
                audioSourceTuc.PlayOneShot(sonidoTuc);
            }

            yield return new WaitForSeconds(velocidadEscritura);
        }

        estaEscribiendo = false;
    }
}