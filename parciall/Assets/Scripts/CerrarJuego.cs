using UnityEngine;

public class CerrarJuego : MonoBehaviour
{
    public void SalirAplicacion()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}