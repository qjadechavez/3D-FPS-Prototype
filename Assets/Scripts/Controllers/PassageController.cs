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
        
        // Make sure gate starts HIDDEN
        if (cubeGate != null)
        {
            cubeGate.SetActive(false); // Start hidden instead of visible
        }
    }
    
    void OnSwitchToggled(string switchID, bool isActivated)
    {
        // Check if this is the switch we care about
        if (switchID == requiredSwitchID && cubeGate != null)
        {
            // Show cube when switch is activated, hide when deactivated
            // When switch is activated, show the cube (gate closes/blocks)
            // When switch is deactivated, hide the cube (gate opens/allows passage)
            cubeGate.SetActive(isActivated);
            
            Debug.Log($"Cube gate {(isActivated ? "closed (visible/blocking)" : "opened (invisible/passage clear)")}");
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