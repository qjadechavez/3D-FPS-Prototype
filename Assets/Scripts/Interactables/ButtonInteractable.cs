using UnityEngine;

public class ButtonInteractable : Interactable
{
    [Header("Button Properties")]
    public string objectiveID;
    [SerializeField] private bool disableAfterObjectiveComplete = false;
    
    protected override void Start()
    {
        base.Start();
        UpdateInteractionMessage();
    }
    
    public override void Interact()
    {
        // Check if objective is already complete
        if (ObjectiveManager.instance != null && ObjectiveManager.instance.IsObjectiveComplete(objectiveID))
        {
            if (disableAfterObjectiveComplete)
            {
                Debug.Log("This objective is already complete!");
                return;
            }
        }
        
        // Progress the objective
        if (ObjectiveManager.instance != null)
        {
            ObjectiveManager.instance.ProgressObjective(objectiveID);
            UpdateInteractionMessage();
        }
        
        // Call base implementation to trigger onInteraction event
        base.Interact();
    }
    
    private void UpdateInteractionMessage()
    {
        if (ObjectiveManager.instance != null)
        {
            var objective = ObjectiveManager.instance.GetObjective(objectiveID);
            if (objective != null)
            {
                if (objective.IsComplete && disableAfterObjectiveComplete)
                {
                    message = "Objective Complete";
                    enabled = false; // Disable this interactable
                }
                else
                {
                    message = $"Press Button ({objective.currentCount}/{objective.targetCount})";
                }
            }
            else
            {
                message = "Press Button";
            }
        }
        else
        {
            message = "Press Button";
        }
    }
    
    private void OnEnable()
    {
        if (ObjectiveManager.instance != null)
        {
            ObjectiveManager.instance.onObjectiveUpdated.AddListener(OnObjectiveUpdated);
        }
    }
    
    private void OnDisable()
    {
        if (ObjectiveManager.instance != null)
        {
            ObjectiveManager.instance.onObjectiveUpdated.RemoveListener(OnObjectiveUpdated);
        }
    }
    
    private void OnObjectiveUpdated(string updatedObjectiveID)
    {
        if (updatedObjectiveID == objectiveID)
        {
            UpdateInteractionMessage();
        }
    }
}