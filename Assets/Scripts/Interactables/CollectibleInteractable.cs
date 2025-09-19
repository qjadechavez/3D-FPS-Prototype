using UnityEngine;

public class CollectibleInteractable : Interactable
{
    [Header("Collectible Properties")]
    public string collectibleID;
    
    protected override void Start()
    {
        base.Start();
        
        // Register with manager
        if (CollectiblesManager.instance != null)
        {
            CollectiblesManager.instance.RegisterCollectible(this);
        }
        
        // Only set default message if no message has been specified
        if (string.IsNullOrEmpty(message))
        {
            UpdateInteractionMessage();
        }
    }
    
    public override void Interact()
    {
        // Notify manager this collectible was collected
        if (CollectiblesManager.instance != null)
        {
            CollectiblesManager.instance.CollectibleCollected(this);
        }
        
        int collected = CollectiblesManager.instance.GetCollectedCount(collectibleID);
        int total = CollectiblesManager.instance.GetTotalCollectibles(collectibleID);
        
        // Update console with progress
        Debug.Log($"Collected {collectibleID} ({collected}/{total})");
        
        // Call base implementation which will trigger onInteraction event
        // (which likely contains gameObject.SetActive(false))
        base.Interact();
    }
    
    private void UpdateInteractionMessage()
    {
        if (CollectiblesManager.instance != null)
        {
            int collected = CollectiblesManager.instance.GetCollectedCount(collectibleID);
            int total = CollectiblesManager.instance.GetTotalCollectibles(collectibleID);
            message = $"Collect {collectibleID} ({collected+1}/{total})";
        }
        else
        {
            message = $"Collect {collectibleID}";
        }
    }
}