using UnityEngine;

public class SwitchableInteractable : Interactable
{
    [Header("Switchable Properties")]
    public string switchID;
    public bool isActivated = false;
    
    [Header("Messages")]
    public string activateMessage = "Activate";
    public string deactivateMessage = "Deactivate";
    
    protected override void Start()
    {
        base.Start();
        
        // Register with manager
        if (SwitchManager.instance != null)
        {
            SwitchManager.instance.RegisterSwitch(this);
        }
        
        // Set initial message
        UpdateInteractionMessage();
    }
    
    public override void Interact()
    {
        // Toggle the switch state
        isActivated = !isActivated;
        
        // Notify manager about state change
        if (SwitchManager.instance != null)
        {
            SwitchManager.instance.SwitchToggled(this, isActivated);
        }
        
        // Update the interaction message
        UpdateInteractionMessage();
        
        // Log the action
        Debug.Log($"{switchID} {(isActivated ? "activated" : "deactivated")}");
        
        // DON'T call base.Interact() - this was causing switches to disappear
        // base.Interact();
    }
    
    public void SetState(bool newState)
    {
        isActivated = newState;
        UpdateInteractionMessage();
    }
    
    private void UpdateInteractionMessage()
    {
        message = isActivated ? deactivateMessage : activateMessage;
        
        // If we have a custom switch ID, include it in the message
        if (!string.IsNullOrEmpty(switchID))
        {
            message = $"{message} {switchID}";
        }
    }
}