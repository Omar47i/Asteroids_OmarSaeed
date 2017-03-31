using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState
{
    Playing,
    MainMenu,
    Revive,
    GameOver,
};

public class GameStateChangedBase : UnityEvent<GameState>
{ }

public class GameManager : MonoBehaviour {

    public static GameManager Instance;        // Make it singleton!

    [HideInInspector]                          // an event to be invoked when he gets out of lives
    public UnityEvent m_GameOverEvent = new UnityEvent();

    [HideInInspector]                          // an event to be invoked when game state changes
    public GameStateChangedBase m_GameStateChangedEvent = new GameStateChangedBase();

    private GameState gameState;
    public GameState m_GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;

            m_GameStateChangedEvent.Invoke(gameState);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)   // duplicate
        {
            Destroy(gameObject);
        }
       
        DontDestroyOnLoad(gameObject);

        m_GameOverEvent.AddListener(OnGameOver);
        m_GameState = GameState.MainMenu;
    }

    private void OnGameOver()
    {
        m_GameState = GameState.GameOver;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
