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
    public static GameManager Singleton { get; private set; }

    public GameState gameState;

    void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogError("GameManager уже существует!");
            return;
        }
        Singleton = this;
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
