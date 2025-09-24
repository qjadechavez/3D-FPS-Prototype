using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchManager : MonoBehaviour
{
    public static SwitchManager instance;
    
    private Dictionary<string, SwitchableInteractable> switchesByID = new Dictionary<string, SwitchableInteractable>();
    private Dictionary<string, bool> switchStates = new Dictionary<string, bool>();
    
    [System.Serializable]
    public class SwitchEvent : UnityEvent<string, bool> { }
    
    [Header("Events")]
    public SwitchEvent onSwitchToggled;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void RegisterSwitch(SwitchableInteractable switchInteractable)
    {
        string id = switchInteractable.switchID;
        
        if (!string.IsNullOrEmpty(id))
        {
            switchesByID[id] = switchInteractable;
            switchStates[id] = switchInteractable.isActivated;
        }
    }
    
    public void SwitchToggled(SwitchableInteractable switchInteractable, bool newState)
    {
        string id = switchInteractable.switchID;
        
        if (!string.IsNullOrEmpty(id))
        {
            switchStates[id] = newState;
            onSwitchToggled?.Invoke(id, newState);
        }
    }
    
    public bool GetSwitchState(string switchID)
    {
        if (switchStates.ContainsKey(switchID))
        {
            return switchStates[switchID];
        }
        return false;
    }
    
    public void SetSwitchState(string switchID, bool state)
    {
        if (switchesByID.ContainsKey(switchID))
        {
            switchesByID[switchID].SetState(state);
            switchStates[switchID] = state;
        }
    }
    
    public bool AllSwitchesActivated(params string[] switchIDs)
    {
        foreach (string id in switchIDs)
        {
            if (!GetSwitchState(id))
            {
                return false;
            }
        }
        return true;
    }
    
    public bool AnySwitchActivated(params string[] switchIDs)
    {
        foreach (string id in switchIDs)
        {
            if (GetSwitchState(id))
            {
                return true;
            }
        }
        return false;
    }
    
    public int GetActivatedCount(params string[] switchIDs)
    {
        int count = 0;
        foreach (string id in switchIDs)
        {
            if (GetSwitchState(id))
            {
                count++;
            }
        }
        return count;
    }
}