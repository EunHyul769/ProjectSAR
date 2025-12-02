using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State { get; private set; }

    public float playTime = 0f;

    private void Awake()
    {
        Instance = this;
        State = GameState.Ready;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (State != GameState.Playing)
            return;

        // Ÿ�̸� ����
        playTime += Time.deltaTime;
        UIManager.Instance.UpdateTimer(playTime);
    }

    public void StartGame()
    {
        State = GameState.Playing;
    }

    public void GameOver()
    {
        State = GameState.GameOver;
        // ���߿� ���â ����
    }
}
