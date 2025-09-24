using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    [Header("Switch Hold Settings")]
    public float switchHoldDuration = 2f; // Time required to hold E for switches
    
    Interactable currentInteractable;
    private bool isHoldingInteract = false;
    private float holdTimer = 0f;
    private bool hasTriggeredInteraction = false;

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        HandleInteractionInput();
    }

    private void HandleInteractionInput()
    {
        if (currentInteractable != null)
        {
            // Check if this is a switch that requires holding E
            if (currentInteractable is SwitchableInteractable)
            {
                // Handle E key for switches
                if (Input.GetKey(KeyCode.E))
                {
                    HandleSwitchHold();
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    ResetHoldTimer();
                }
            }
            else
            {
                // Handle F key for collectibles (immediate interaction)
                if (Input.GetKeyDown(KeyCode.F))
                {
                    currentInteractable.Interact();
                }
            }
        }
        else
        {
            // No interactable in range, reset timer
            ResetHoldTimer();
        }
    }

    private void HandleSwitchHold()
    {
        if (!isHoldingInteract)
        {
            isHoldingInteract = true;
            holdTimer = 0f;
            hasTriggeredInteraction = false;
            Debug.Log("Hold E to activate switch...");
        }

        holdTimer += Time.deltaTime;
        
        // Show progress in console every 0.5 seconds
        if (Mathf.FloorToInt(holdTimer * 2) != Mathf.FloorToInt((holdTimer - Time.deltaTime) * 2))
        {
            float progress = (holdTimer / switchHoldDuration) * 100f;
            Debug.Log($"Switch activation: {progress:F0}%");
        }

        // Check if hold duration is complete
        if (holdTimer >= switchHoldDuration && !hasTriggeredInteraction)
        {
            hasTriggeredInteraction = true;
            currentInteractable.Interact();
            Debug.Log("Switch activated!");
            ResetHoldTimer();
        }
    }

    private void ResetHoldTimer()
    {
        isHoldingInteract = false;
        holdTimer = 0f;
        hasTriggeredInteraction = false;
    }

    private bool IsInteractableTag(string tag)
    {
        return tag == "CollectibleInteractable" || tag == "SwitchableInteractable";
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (IsInteractableTag(hit.collider.tag))
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();
                if (currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                    ResetHoldTimer(); // Reset when switching targets
                }

                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else // if new interactable is not enabled
                {
                    DisableCurrentInteractable();
                }
            }
            else // if not an interactable
            {
                DisableCurrentInteractable();
            }
        }
        else // if not in reach of any interactable
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        
        // Show different messages for switches vs collectibles with appropriate keys
        string displayMessage = currentInteractable.message;
        if (currentInteractable is SwitchableInteractable)
        {
            displayMessage += $" (Hold E for {switchHoldDuration}s)";
        }
        else
        {
            displayMessage += " (Press F)";
        }
        
        HUDController.instance.EnableInteractionText(displayMessage);
    }
    
    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
            ResetHoldTimer(); // Reset when no target
        }
    }
}
