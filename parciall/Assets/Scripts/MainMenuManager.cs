using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneles de la UI")]
    public GameObject panelMainMenu;
    public GameObject panelExplicacion;

    [Header("Nombre de la Escena 3D")]
    public string nombreEscenaJuego = "PruebasCata"; // Debe llamarse exacto como tu escena 3D

    private void Start()
    {
        MostrarMainMenu();
    }

    public void MostrarMainMenu()
    {
        panelMainMenu.SetActive(true);
        panelExplicacion.SetActive(false);
    }

    public void MostrarExplicacion()
    {
        panelMainMenu.SetActive(false);
        panelExplicacion.SetActive(true);
    }

    public void CargarJuego()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }
}