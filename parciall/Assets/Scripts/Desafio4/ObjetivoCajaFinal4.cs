using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class ObjetivoCajaFinal4 : MonoBehaviour
{
    [Header("Llave que aparecerá")]
    public GameObject llave;

    [Header("Jaula que desaparecerá")]
    public GameObject jaula;

    private bool completado = false;

    public AudioSource sonidoActivacion;

    private void OnTriggerEnter(Collider other)
    {
        if (completado)
            return;

        if (other.CompareTag("Caja"))
        {
            completado = true;

            // Desaparece la caja
            other.gameObject.SetActive(false);

            // Aparece la llave
            if (llave != null)
            {
                llave.SetActive(true);
            }

            Debug.Log("La caja llegó al final");
            Debug.Log("¡Apareció la llave!");

            // Iniciar la secuencia de la jaula y cambio de escena
            StartCoroutine(SecuenciaFinal());

            if (sonidoActivacion != null)
                sonidoActivacion.Play();
        }
    }

    private IEnumerator SecuenciaFinal()
    {
        // 1. Esperar 2 segundos y desaparecer la jaula
        yield return new WaitForSeconds(2f);

        if (jaula != null)
        {
            jaula.SetActive(false);
        }

        Debug.Log("¡La jaula desapareció!");

        // 2. Esperar otros 2 segundos y cargar la escena Final
        yield return new WaitForSeconds(2f);

        Debug.Log("Cargando escena Final...");
        SceneManager.LoadScene("Final");
    }
}