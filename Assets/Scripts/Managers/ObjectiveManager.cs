using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    
    [System.Serializable]
    public class Objective
    {
        public string objectiveID;
        public int targetCount;
        public int currentCount;
        public string description;
        public UnityEvent onObjectiveComplete;
        
        public bool IsComplete => currentCount >= targetCount;
        public float Progress => (float)currentCount / targetCount;
    }
    
    [SerializeField] private List<Objective> objectives = new List<Objective>();
    private Dictionary<string, Objective> objectiveDict = new Dictionary<string, Objective>();
    
    public UnityEvent<string> onObjectiveUpdated;
    public UnityEvent<string> onObjectiveCompleted;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeObjectives();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeObjectives()
    {
        foreach (var objective in objectives)
        {
            objectiveDict[objective.objectiveID] = objective;
        }
    }
    
    public void ProgressObjective(string objectiveID)
    {
        if (objectiveDict.ContainsKey(objectiveID))
        {
            var objective = objectiveDict[objectiveID];
            if (!objective.IsComplete)
            {
                objective.currentCount++;
                onObjectiveUpdated.Invoke(objectiveID);
                
                Debug.Log($"{objective.description}: {objective.currentCount}/{objective.targetCount}");
                
                if (objective.IsComplete)
                {
                    Debug.Log($"Objective Complete: {objective.description}");
                    objective.onObjectiveComplete.Invoke();
                    onObjectiveCompleted.Invoke(objectiveID);
                }
            }
        }
    }
    
    public Objective GetObjective(string objectiveID)
    {
        return objectiveDict.ContainsKey(objectiveID) ? objectiveDict[objectiveID] : null;
    }
    
    public bool IsObjectiveComplete(string objectiveID)
    {
        var objective = GetObjective(objectiveID);
        return objective != null && objective.IsComplete;
    }
}