using UnityEngine;

public class CubeBarrierController : MonoBehaviour
{
    [Header("Gate Settings")]
    // Reference to the cube gate GameObject (Passages)
    public GameObject cubeGate; 
    public string[] requiredSwitchIDs = {"Switch", "Switch1", "Switch2", "Switch3"};
    
    void Start()
    {
        // Subscribe to switch events from SwitchManager
        if (SwitchManager.instance != null)
        {
            SwitchManager.instance.onSwitchToggled.AddListener(OnSwitchToggled);
        }
        
        // Set initial state of the passage to inactive (hidden)
        if (cubeGate != null)
        {
            cubeGate.SetActive(false);
        }
    }
    
    void OnSwitchToggled(string switchID, bool isActivated)
    {
        // Check if the toggled switch is one of the relevant switches
        bool isRelevantSwitch = false;
        foreach (string requiredID in requiredSwitchIDs)
        {
            if (switchID == requiredID)
            {
                isRelevantSwitch = true;
                break;
            }
        }
        
        // If it's a relevant switch, check the states of all required switches
        if (isRelevantSwitch && cubeGate != null)
        {
            bool allSwitchesActivated = SwitchManager.instance.AllSwitchesActivated(requiredSwitchIDs);

            // Set the cube gate active if all required switches are activated
            cubeGate.SetActive(allSwitchesActivated);

            Debug.Log($"Switches status: {SwitchManager.instance.GetActivatedCount(requiredSwitchIDs)}/{requiredSwitchIDs.Length} activated. Stairs {(allSwitchesActivated ? "VISIBLE" : "INVISIBLE")}");
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (SwitchManager.instance != null)
        {
            SwitchManager.instance.onSwitchToggled.RemoveListener(OnSwitchToggled);
        }
    }
}