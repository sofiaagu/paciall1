using UnityEngine;

public class MecanismoDesafio1 : MonoBehaviour
{
    public enum TipoMecanismo
    {
        Mono,
        Paloma,
        Serpiente
    }

    public TipoMecanismo tipo;

    public Desafio1Manager manager;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (tipo == TipoMecanismo.Mono && other.CompareTag("Mono"))
        {
            activado = true;
            manager.monoActivo = true;

            Debug.Log("Mono activó su mecanismo");
        }

        if (tipo == TipoMecanismo.Paloma && other.CompareTag("Paloma"))
        {
            activado = true;
            manager.palomaActiva = true;

            Debug.Log("Paloma activó su mecanismo");
        }

        if (tipo == TipoMecanismo.Serpiente && other.CompareTag("Serpiente"))
        {
            activado = true;
            manager.serpienteActiva = true;

            Debug.Log("Serpiente activó su mecanismo");
        }
    }
}
