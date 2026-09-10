using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(PlayerInteraction player)
    {
        Debug.Log(gameObject.name + " fue interactuado.");
    }

    public virtual void StopInteract(PlayerInteraction player)
    {
    }
}