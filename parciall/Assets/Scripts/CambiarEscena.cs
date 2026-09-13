using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    // Escribe exactamente el nombre de tu escena de juego (por ejemplo, "Nivel1")
    public string nombreEscenaJuego;

    public void IrAEscenaJuego()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }
}