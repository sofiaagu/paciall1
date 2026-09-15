using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [Header("Nombre de la siguiente escena")]
    public string sceneToLoad = "nivel 2";

    [Header("Jugadores requeridos")]
    public Transform[] players;

    private HashSet<Transform> playersInFinishZone = new HashSet<Transform>();
    private bool levelCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (levelCompleted) return;

        foreach (Transform player in players)
        {
            if (player != null && (other.transform == player || other.transform.IsChildOf(player)))
            {
                playersInFinishZone.Add(player);
                Debug.Log($"Jugador en la meta: {player.name} ({playersInFinishZone.Count}/{players.Length})");
                CheckLevelCompletion();
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (levelCompleted) return;

        foreach (Transform player in players)
        {
            if (player != null && (other.transform == player || other.transform.IsChildOf(player)))
            {
                playersInFinishZone.Remove(player);
                break;
            }
        }
    }

    private void CheckLevelCompletion()
    {
        if (playersInFinishZone.Count >= players.Length)
        {
            levelCompleted = true;
            Debug.Log("<color=green>¡Nivel completado! Cargando escena: " + sceneToLoad + "</color>");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}