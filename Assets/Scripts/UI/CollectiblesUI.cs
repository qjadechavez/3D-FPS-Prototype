using UnityEngine;
using TMPro;

public class CollectiblesUI : MonoBehaviour
{
    [SerializeField] private TMP_Text collectiblesCountText;
    [SerializeField] private string collectibleID;
    
    void Update()
    {
        if (CollectiblesManager.instance != null && collectiblesCountText != null)
        {
            int collected = CollectiblesManager.instance.GetCollectedCount(collectibleID);
            int total = CollectiblesManager.instance.GetTotalCollectibles(collectibleID);
            
            collectiblesCountText.text = $"{collectibleID}s : {collected}/{total}";
        }
    }
}