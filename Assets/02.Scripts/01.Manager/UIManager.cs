using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD")]
    public Slider hpBar;
    public Slider expBar;
    public TMP_Text timerText;

    [Header("HUD Slots")]
    public Transform weaponSlotParent;
    public Transform equipmentSlotParent;

    public GameObject weaponSlotPrefab;
    public GameObject equipmentSlotPrefab;

    [Header("Panels")]
    public LevelUpPanel levelUpPanel;
    public SkillChoicePanel skillChoicePanel;
    public PausePanel pausePanel;
    public GameOverPanel gameOverPanel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        CreateSlots(weaponSlotParent, weaponSlotPrefab, 6);
        CreateSlots(equipmentSlotParent, equipmentSlotPrefab, 6);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SkillChoicePanel.Instance != null &&
                    SkillChoicePanel.Instance.isOpen)
                    return;

                if (LevelUpPanel.Instance != null &&
                    LevelUpPanel.Instance.isOpen)
                    return;

                TogglePause();
            }
        }
    }
    private void CreateSlots(Transform parent, GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab, parent);
        }
    }
    public void UpdateHP(float current, float max)
    {
        if (hpBar == null) return;
        hpBar.value = current / max;
    }

    public void UpdateEXP(float current, float max)
    {
        if (expBar == null) return;
        expBar.value = current / max;
    }
    public void UpdateTimer(float time)
    {
        int min = (int)(time / 60);
        int sec = (int)(time % 60);
        timerText.text = $"{min:00}:{sec:00}";
    }
    // 레벨업 패널 열기
    public void OpenLevelUp(LevelUpOptionData[] options)
    {
        LevelUpPanel.Instance.Open(options);
    }
    // 스킬 선택 패널 열기
    public void OpenSkillChoice(SkillData[] datas)
    {
        skillChoicePanel.Open(datas);
    }
    // 일시정지 패널 열기
    public void TogglePause()
    {
        if (pausePanel.isOpen)
            pausePanel.Close();
        else
            pausePanel.Open();
    }
    // 게임오버 패널 열기
    public void OpenGameOver(string playtime)
    {
        gameOverPanel.Open(playtime);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
