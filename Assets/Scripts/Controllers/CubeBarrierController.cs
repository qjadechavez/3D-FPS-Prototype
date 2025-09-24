using UnityEngine;

public class CubeBarrierController : MonoBehaviour
{
    [Header("Gate Settings")]
    public GameObject cubeGate; // The cube that acts as the gate
    public string requiredSwitchID = "Switch"; // Which switch controls this gate
    
    void Start()
    {
        // Subscribe to switch events from SwitchManager
        if (SwitchManager.instance != null)
        {
            SwitchManager.instance.onSwitchToggled.AddListener(OnSwitchToggled);
        }
        
        // Make sure gate starts visible
        if (cubeGate != null)
        {
            cubeGate.SetActive(true);
        }
    }
    
    void OnSwitchToggled(string switchID, bool isActivated)
    {
        // Check if this is the switch we care about
        if (switchID == requiredSwitchID && cubeGate != null)
        {
            // Toggle cube visibility based on switch state
            // When switch is activated, hide the cube (gate opens)
            // When switch is deactivated, show the cube (gate closes)
            cubeGate.SetActive(!isActivated);
            
            Debug.Log($"Cube gate {(isActivated ? "opened (invisible)" : "closed (visible)")}");
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