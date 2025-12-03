using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Skill Data (ScriptableObjects)")]
    public SkillData startingSkill1;
    public SkillData startingSkill2;

    public SkillData ultimateSkillSO;

    public SkillData normalSkillA;
    public SkillData normalSkillB;

    public GameState State { get; private set; }
    public float playTime = 0f;

    // 테스트용 아이콘
    public Sprite testItemIcon;

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
            UIManager.Instance.UpdateTimer(playTime);
    }

    // 게임 시작 시 최초 스킬 선택
    public void StartGame()
    {
        State = GameState.Playing;

        SkillData[] startingSkills = new SkillData[2];

        // ★ 여기는 네가 만든 SkillData SO를 넣어야 함
        startingSkills[0] = startingSkill1;  // ScriptableObject 참조
        startingSkills[1] = startingSkill2;

        UIManager.Instance.OpenSkillChoice(startingSkills);
    }

    //레벨업 발생 시 호출됨 (PlayerExp에서 level 넘김)
    public void OnPlayerLevelUp(int level)
    {
        if (level == 20)
        {
            var ult = CreateUltimateSkill();
            UIManager.Instance.OpenSkillChoice(ult);
            return;
        }

        if (level == 40)
        {
            var normals = CreateNormalSkills();
            UIManager.Instance.OpenSkillChoice(normals);
            return;
        }

        var items = CreateItemOptions();
        UIManager.Instance.OpenLevelUp(items);
    }

    // 궁극기 스킬 1개 (20레벨)
    private SkillData[] CreateUltimateSkill()
    {
        return new SkillData[]
        {
            ultimateSkillSO
        };
    }
    private SkillData[] CreateNormalSkills()
    {
        return new SkillData[]
        {
        normalSkillA,
        normalSkillB
        };
    }

    // 아이템 / 무기 / 장비 3개 옵션 (기본 레벨업)
    private LevelUpOptionData[] CreateItemOptions()
    {
        return new LevelUpOptionData[]
        {
            new LevelUpOptionData()
            {
                icon = testItemIcon,
                name = "단검",
                metaInfo = "언커먼 / 무기 / 보유 1개",
                description = "Lv 2 → Lv 3\n공격력 +10%",
                rarityColor = Color.green
            },
            new LevelUpOptionData()
            {
                icon = testItemIcon,
                name = "방패",
                metaInfo = "레어 / 장비 / 보유 0개",
                description = "Lv 1 → Lv 2\n방어력 +15",
                rarityColor = Color.blue
            },
            new LevelUpOptionData()
            {
                icon = testItemIcon,
                name = "부츠",
                metaInfo = "커먼 / 장비 / 보유 1개",
                description = "Lv 1 → Lv 2\n이동속도 +5%",
                rarityColor = Color.white
            }
        };
    }

    public void GameOver()
    {
        if (State == GameState.GameOver)
            return;

        State = GameState.GameOver;

        int min = (int)(playTime / 60);
        int sec = (int)(playTime % 60);
        string playtimeStr = $"{min:00}:{sec:00}";

        UIManager.Instance.OpenGameOver(playtimeStr);

        Debug.Log("게임 오버!");
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
