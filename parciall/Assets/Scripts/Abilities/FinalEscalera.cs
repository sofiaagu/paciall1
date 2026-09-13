using UnityEngine;

public class FinalEscalera : MonoBehaviour
{
    [Header("Punto donde aparecerá el Mono")]
    public Transform puntoSalida;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Mono"))
            return;

        MonoEscalar mono = other.GetComponent<MonoEscalar>();

        if (mono != null)
        {
            mono.SalirPorArriba(puntoSalida);

            Debug.Log("🐒 El Mono llegó al final de la escalera");
        }
    }
}