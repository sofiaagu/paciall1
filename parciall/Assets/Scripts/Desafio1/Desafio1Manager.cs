using UnityEngine;

public class Desafio1Manager : MonoBehaviour
{
    [Header("Mecanismos")]
    public bool monoActivo;
    public bool palomaActiva;
    public bool serpienteActiva;

    [Header("Caja")]
    public bool cajaEnPosicion;

    [Header("Roca")]
    public GameObject roca;

    private bool desafioCompletado = false;

    void Update()
    {
        ComprobarDesafio();
    }

    void ComprobarDesafio()
    {
        if (desafioCompletado)
            return;

        if (monoActivo &&
            palomaActiva &&
            serpienteActiva &&
            cajaEnPosicion)
        {
            CompletarDesafio();
        }
    }

    void CompletarDesafio()
    {
        desafioCompletado = true;

        Debug.Log("¡DESAFÍO 1 COMPLETADO!");

        if (roca != null)
        {
            roca.SetActive(false);
        }
    }
}