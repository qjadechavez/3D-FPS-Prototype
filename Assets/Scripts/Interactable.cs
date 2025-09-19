using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    Outline outline;
    public string message;

    public UnityEvent onInteraction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();

    }

    // Interact is called when the player interacts with the object
    public void Interact()
    {
        onInteraction.Invoke();

    }

    // Enables or disables the outline component
    public void DisableOutline()
    {
        outline.enabled = false;
    }
    
    public void EnableOutline()
    {
        outline.enabled = true;
    }
}
