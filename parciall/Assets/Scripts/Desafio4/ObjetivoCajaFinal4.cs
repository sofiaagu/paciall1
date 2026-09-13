using UnityEngine;
using System.Collections;

public class ObjetivoCajaFinal4 : MonoBehaviour
{
    [Header("Llave que aparecerá")]
    public GameObject llave;

    [Header("Jaula que desaparecerá")]
    public GameObject jaula;

    private bool completado = false;

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

            Debug.Log("📦 La caja llegó al final");
            Debug.Log("🔑 ¡Apareció la llave!");

            // Esperar 2 segundos y desaparecer la jaula
            StartCoroutine(DesaparecerJaula());
        }
    }

    private IEnumerator DesaparecerJaula()
    {
        yield return new WaitForSeconds(2f);

        if (jaula != null)
        {
            jaula.SetActive(false);
        }

        Debug.Log("🔓 ¡La jaula desapareció!");
    }
}