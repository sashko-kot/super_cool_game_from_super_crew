using UnityEngine;

public enum GameState
{
    StartGame,
    BoardGame,
    MiniGame,
    FinishGame
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState gameState;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GameManager уже существует!");
            return;
        }
        Instance = this;
    }

    void Start()
    {
        //gameState = GameState.StartGame;
    }

    void Update()
    {
        // глобальная логика игры
    }

    void OnGui()
    {
        // глобальная логика интерфейса
    }
}
