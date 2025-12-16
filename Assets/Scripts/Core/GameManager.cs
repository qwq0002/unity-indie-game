using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    Dialogue
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
    public System.Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        // 处理通用的逻辑
        switch (newState)
        {
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Dialogue:
                Time.timeScale = 0f; // 对话时暂停游戏
                break;
        }

        OnStateChanged?.Invoke(newState);
    }

    public void StartNewGame()
    {
        // 重置存档并开始第一关
        SaveManager.Instance.ResetProgress();
        LoadLevel("Level_1_1");
    }

    public void ContinueGame()
    {
        // 加载最后游玩的关卡
        string lastLevel = SaveManager.Instance.GetLastPlayedLevel();
        LoadLevel(lastLevel);
    }

    public void LoadLevel(string sceneName)
    {
        ChangeState(GameState.Playing);
        SceneManager.LoadScene(sceneName);
    }

    public void ReturnToMainMenu()
    {
        ChangeState(GameState.MainMenu);
        SceneManager.LoadScene("MainMenu");
    }
}
