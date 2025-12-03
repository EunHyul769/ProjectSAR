using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    public SkillData ultimateSkillSO;          // 궁극기
    public SkillData[] normalSkillPool;        // 일반 스킬 풀

    // 이미 선택된 일반 스킬 리스트 (중복 방지)
    private List<SkillData> ownedNormalSkills = new List<SkillData>();

    public GameState State { get; private set; }
    public float playTime = 0f;

    [Header("Test Icons (LevelUp Dummy)")]
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

    // 게임 시작 시 첫 스킬 2개 랜덤
    public void StartGame()
    {
        State = GameState.Playing;

        // 랜덤 일반 스킬 2개
        SkillData[] startingSkills = GetRandomNormalSkills(2);

        UIManager.Instance.OpenSkillChoice(startingSkills);
    }

    // 외부에서 호출되는: 스킬 선택 시 선택된 스킬 저장
    public void AddOwnedNormalSkill(SkillData data)
    {
        if (data == null) return;
        if (!ownedNormalSkills.Contains(data))
            ownedNormalSkills.Add(data);
    }

    // 중복 제거 + 랜덤 일반 스킬 뽑기
    private SkillData[] GetRandomNormalSkills(int count)
    {
        if (normalSkillPool == null || normalSkillPool.Length == 0)
            return new SkillData[0];

        // 이미 선택한 스킬 제외한 풀 생성
        List<SkillData> available = new List<SkillData>();

        foreach (var s in normalSkillPool)
        {
            if (!ownedNormalSkills.Contains(s))
                available.Add(s);
        }

        if (available.Count == 0)
            return new SkillData[0];

        // 셔플
        for (int i = 0; i < available.Count; i++)
        {
            int rand = Random.Range(0, available.Count);
            (available[i], available[rand]) = (available[rand], available[i]);
        }

        // 필요한 개수만큼 가져오기
        int pick = Mathf.Min(count, available.Count);
        SkillData[] result = new SkillData[pick];

        for (int i = 0; i < pick; i++)
            result[i] = available[i];

        return result;
    }

    // 레벨업 발생 시 호출됨
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
            // 40레벨도 랜덤 일반 스킬 2개
            var normals = GetRandomNormalSkills(2);
            UIManager.Instance.OpenSkillChoice(normals);
            return;
        }

        // 일반 아이템/무기/장비 레벨업 UI
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

    // 기본 레벨업 아이템 3개
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
