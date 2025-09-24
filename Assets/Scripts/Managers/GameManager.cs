using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Game State")]
    public int currentLevel = 1;
    public GameState gameState = GameState.Playing;
    
    [Header("Managers References")]
    public CollectiblesManager collectiblesManager;
    public ObjectiveManager objectiveManager; // You'll create this too
    
    [Header("Game Events")]
    public UnityEvent onLevelComplete;
    public UnityEvent onGameComplete;
    public UnityEvent<GameState> onGameStateChanged;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void InitializeGame()
    {
        // Get references to existing managers
        if (collectiblesManager == null)
            collectiblesManager = CollectiblesManager.instance;
            
        // Subscribe to manager events when ObjectiveManager is created
        // if (objectiveManager != null)
        // {
        //     objectiveManager.onObjectiveCompleted.AddListener(OnObjectiveCompleted);
        // }
        
        SetGameState(GameState.Playing);
    }
    
    public void SetGameState(GameState newState)
    {
        gameState = newState;
        onGameStateChanged.Invoke(newState);
        
        switch (newState)
        {
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
        }
    }
    
    // Central access to all game data
    public int GetCollectedItems(string itemType)
    {
        return collectiblesManager?.GetCollectedCount(itemType) ?? 0;
    }
}

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    LevelComplete
}