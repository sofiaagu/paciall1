using System.Collections;
using UnityEngine;
using TMPro;

public class HistoriaManager : MonoBehaviour
{
    [System.Serializable]
    public struct LineaDialogo
    {
        public GameObject globoDialogo; // El objeto Globo (ej. Globo_mono)
        public TextMeshProUGUI textoTMP;  // El componente TextMeshPro dentro del globo
        [TextArea(2, 4)]
        public string textoCompleto;    // El texto que aparecerá
    }

    [Header("Configuración de Diálogos")]
    public LineaDialogo[] lineas;
    public float velocidadEscritura = 0.04f;

    [Header("Audio SFX")]
    public AudioSource audioSourceTuc; 
    public AudioClip sonidoTuc;        

    [Header("UI")]
    public GameObject botonSiguiente;

    private int indiceActual = 0;
    private bool estaEscribiendo = false;

    void OnEnable()
    {
        // Se ejecuta cada vez que el panel se activa (Panel 1 o Panel 2)
        ReiniciarYEmpezar();
    }

    void ReiniciarYEmpezar()
    {
        StopAllCoroutines();
        indiceActual = 0;
        estaEscribiendo = false;

        // Limpiar y apagar todos los globos al inicio
        foreach (var linea in lineas)
        {
            if (linea.globoDialogo != null)
                linea.globoDialogo.SetActive(false);

            if (linea.textoTMP != null)
                linea.textoTMP.text = ""; // Borra cualquier letra basura como la 'H'
        }

        if (botonSiguiente != null)
            botonSiguiente.SetActive(false);

        // Iniciar el primer diálogo si existen líneas
        if (lineas != null && lineas.Length > 0)
        {
            StartCoroutine(EscribirLinea(0));
        }
    }

    public void AvanzarDialogo()
    {
        if (lineas == null || lineas.Length == 0) return;

        // Si se sobrepasó el índice, activa el botón y sale
        if (indiceActual >= lineas.Length)
        {
            if (botonSiguiente != null)
                botonSiguiente.SetActive(true);
            return;
        }

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
            // Fin de la historia de este panel: Mostrar botón Siguiente
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

        if (linea.textoTMP != null)
            linea.textoTMP.text = "";

        foreach (char letra in linea.textoCompleto.ToCharArray())
        {
            if (linea.textoTMP != null)
                linea.textoTMP.text += letra;

            if (letra != ' ' && audioSourceTuc != null && sonidoTuc != null)
            {
                audioSourceTuc.PlayOneShot(sonidoTuc);
            }

            yield return new WaitForSeconds(velocidadEscritura);
        }

        estaEscribiendo = false;
    }
}