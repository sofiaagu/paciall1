using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneles de la UI")]
    public GameObject panelMainMenu;
    public GameObject panelExplicacion;
    public GameObject panelHistorieta; // Nueva casilla para el panel de la historieta

    [Header("Nombre de la Escena 3D")]
    public string nombreEscenaJuego = "PruebasCata"; 

    private void Start()
    {
        MostrarMainMenu();
    }

    public void MostrarMainMenu()
    {
        if (panelMainMenu != null) panelMainMenu.SetActive(true);
        if (panelExplicacion != null) panelExplicacion.SetActive(false);
        if (panelHistorieta != null) panelHistorieta.SetActive(false);
    }

    public void MostrarExplicacion()
    {
        if (panelMainMenu != null) panelMainMenu.SetActive(false);
        if (panelExplicacion != null) panelExplicacion.SetActive(true);
        if (panelHistorieta != null) panelHistorieta.SetActive(false);
    }

    public void MostrarHistorieta()
    {
        if (panelMainMenu != null) panelMainMenu.SetActive(false);
        if (panelExplicacion != null) panelExplicacion.SetActive(false);
        if (panelHistorieta != null) panelHistorieta.SetActive(true);
    }

    public void CargarJuego()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }
}