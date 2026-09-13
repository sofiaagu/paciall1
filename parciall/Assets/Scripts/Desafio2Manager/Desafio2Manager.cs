using UnityEngine;

public class Desafio2Manager : MonoBehaviour
{
    [Header("Posiciones iniciales")]
    public bool cajaEnPosicion;
    public bool monoEnPosicion;
    public bool palomaEnPosicion;

    [Header("Estado del puente")]
    public bool puentePreparado;
    public bool puenteEstabilizado;

    [Header("Puentes")]
    public GameObject puenteInestable;
    public GameObject puenteEstable;

    private void Update()
    {
        ComprobarPreparacion();
    }

    private void ComprobarPreparacion()
    {
        // Si ya está completamente estabilizado,
        // no necesitamos volver a comprobar nada.
        if (puenteEstabilizado)
            return;

        // Los tres personajes y la caja deben estar
        // en sus posiciones al mismo tiempo.
        if (cajaEnPosicion &&
            monoEnPosicion &&
            palomaEnPosicion)
        {
            PrepararPuente();
        }
        else
        {
            puentePreparado = false;
        }
    }

    private void PrepararPuente()
    {
        if (puentePreparado)
            return;

        puentePreparado = true;

        Debug.Log("Los tres están en posición. El puente está temporalmente estable.");
    }

    public void EstabilizarPuente()
    {
        if (!puentePreparado)
            return;

        puenteEstabilizado = true;

        Debug.Log("¡La serpiente activó el botón! El puente quedó completamente estable.");

        if (puenteInestable != null)
        {
            puenteInestable.SetActive(false);
        }

        if (puenteEstable != null)
        {
            puenteEstable.SetActive(true);
        }
    }
}