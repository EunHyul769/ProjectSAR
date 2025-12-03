using Unity.VisualScripting;
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
        Time.timeScale = 1f;

        StartGame();
    }

    private void Update()
    {
        if (State != GameState.Playing)
            return;

        playTime += Time.deltaTime;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimer(playTime);
        }
    }

    public void StartGame()
    {
        State = GameState.Playing;

        // 더미데이터
        SkillOptionData[] startingOptions = new SkillOptionData[2];

        startingOptions[0] = new SkillOptionData()
        {
            name = "스타팅 스킬 1",
            description = "시작 스킬 설명 1",
            skillType = SkillType.Normal
        };

        startingOptions[1] = new SkillOptionData()
        {
            name = "스타팅 스킬 2",
            description = "시작 스킬 설명 2",
            skillType = SkillType.Normal
        };

        UIManager.Instance.OpenSkillChoice(startingOptions);
    }

    public void GameOver()
    {
        State = GameState.GameOver;
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
