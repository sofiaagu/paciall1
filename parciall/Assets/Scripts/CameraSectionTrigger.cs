using UnityEngine;

public class CameraSectionTrigger : MonoBehaviour
{
    [Header("Cámara")]
    public CoopCamera coopCamera;

    [Header("Siguiente sección")]
    public Transform nextSection;

    [Header("Jugadores")]
    public Transform[] players;

    private bool[] playersInside;

    private bool sectionActivated;

    private void Start()
    {
        playersInside = new bool[players.Length];
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (other.transform == players[i])
            {
                playersInside[i] = true;

                CheckAllPlayersInside();

                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (other.transform == players[i])
            {
                playersInside[i] = false;

                return;
            }
        }
    }

    private void CheckAllPlayersInside()
    {
        if (sectionActivated)
            return;

        for (int i = 0; i < playersInside.Length; i++)
        {
            if (!playersInside[i])
                return;
        }

        sectionActivated = true;

        coopCamera.ChangeSection(nextSection);
    }
}