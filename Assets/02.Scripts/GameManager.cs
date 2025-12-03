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

        // 게임 시작 → 스킬 선택창
        UIManager.Instance.OpenSkillChoice(startingOptions);
    }

    //레벨업 발생 시 호출됨 (PlayerExp에서 level 넘김)
    public void OnPlayerLevelUp(int level)
    {
        // 20레벨 → 궁극기 스킬 선택
        if (level == 20)
        {
            var ultimate = CreateUltimateSkillOption();
            UIManager.Instance.OpenSkillChoice(ultimate);
            return;
        }

        // 40레벨 → 일반 스킬 2번째 선택
        if (level == 40)
        {
            var normals = CreateNormalSkillOptions();
            UIManager.Instance.OpenSkillChoice(normals);
            return;
        }

        // 그 외 → 아이템 / 무기 / 장비 카드
        var items = CreateItemOptions();
        UIManager.Instance.OpenLevelUp(items);
    }

    // 궁극기 스킬 1개 (20레벨)
    private SkillOptionData[] CreateUltimateSkillOption()
    {
        return new SkillOptionData[]
        {
            new SkillOptionData()
            {
                name = "궁극기 스킬",
                description = "강력한 충격파를 발사합니다.",
                skillType = SkillType.Ultimate
            }
        };
    }

    // 일반 스킬 2개 (40레벨)
    private SkillOptionData[] CreateNormalSkillOptions()
    {
        return new SkillOptionData[]
        {
            new SkillOptionData()
            {
                name = "스킬 A",
                description = "일반 스킬 설명 A",
                skillType = SkillType.Normal
            },
            new SkillOptionData()
            {
                name = "스킬 B",
                description = "일반 스킬 설명 B",
                skillType = SkillType.Normal
            }
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
