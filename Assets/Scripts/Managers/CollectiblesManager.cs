using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    public static CollectiblesManager instance;
    
    private Dictionary<string, List<CollectibleInteractable>> collectiblesByType = new Dictionary<string, List<CollectibleInteractable>>();
    private Dictionary<string, int> collectedCountByType = new Dictionary<string, int>();
    
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
    
    public void RegisterCollectible(CollectibleInteractable collectible)
    {
        string id = collectible.collectibleID;
        
        // Initialize lists if they don't exist
        if (!collectiblesByType.ContainsKey(id))
        {
            collectiblesByType[id] = new List<CollectibleInteractable>();
            collectedCountByType[id] = 0;
        }
        
        // Add to list
        collectiblesByType[id].Add(collectible);
    }
    
    public void CollectibleCollected(CollectibleInteractable collectible)
    {
        string id = collectible.collectibleID;
        
        if (collectedCountByType.ContainsKey(id))
        {
            collectedCountByType[id]++;
        }
    }
    
    public int GetTotalCollectibles(string collectibleID)
    {
        if (collectiblesByType.ContainsKey(collectibleID))
        {
            return collectiblesByType[collectibleID].Count;
        }
        return 0;
    }
    
    public int GetCollectedCount(string collectibleID)
    {
        if (collectedCountByType.ContainsKey(collectibleID))
        {
            return collectedCountByType[collectibleID];
        }
        return 0;
    }
    
    public int GetRemainingCount(string collectibleID)
    {
        return GetTotalCollectibles(collectibleID) - GetCollectedCount(collectibleID);
    }
}