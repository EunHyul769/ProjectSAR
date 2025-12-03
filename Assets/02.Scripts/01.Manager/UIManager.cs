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

    [Header("HUD Text")]
    public TMP_Text hpText;
    public TMP_Text expText;

    [Header("Panels")]
    public LevelUpPanel levelUpPanel;
    public SkillChoicePanel skillChoicePanel;
    public PausePanel pausePanel;
    public GameOverPanel gameOverPanel;

    [Header("Skill / Dash Slot")]
    public MultiSlotUI dashSlot;
    public MultiSlotUI slotZ;
    public MultiSlotUI slotX;
    public MultiSlotUI slotC;

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
        // 대쉬 슬롯 초기 세팅 추가
        var player = FindObjectOfType<BaseController>();
        if (player != null)
        {
            dashSlot.SetSkill(
                dashSlot.icon.sprite,
                GetDashCooldown(player),
                "Shift"
            );
        }
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

        if (hpText != null)
            hpText.text = $"{(int)current} / {(int)max}";
    }

    public void UpdateEXP(float current, float max)
    {
        if (expBar == null) return;
        expBar.value = current / max;

        if (expText != null)
        {
            float percent = (current / max) * 100f;
            expText.text = $"{percent:0}%";
        }
    }
    public void UpdateTimer(float time)
    {
        int min = (int)(time / 60);
        int sec = (int)(time % 60);
        timerText.text = $"{min:00}:{sec:00}";
    }
    public void OpenLevelUp(LevelUpOptionData[] options)
    {
        LevelUpPanel.Instance.Open(options);
    }
    public void OpenSkillChoice(SkillData[] datas)
    {
        skillChoicePanel.Open(datas);
    }
    public void TogglePause()
    {
        if (pausePanel.isOpen)
            pausePanel.Close();
        else
            pausePanel.Open();
    }
    public void OpenGameOver(string playtime)
    {
        gameOverPanel.Open(playtime);
    }
    public void OnDashUsed()
    {
        dashSlot.StartCooldown();
    }
    float GetDashCooldown(BaseController player)
    {
        return player.GetDashCooldown();
    }
    public void SetSkillToHUD(SkillData data)
    {
        MultiSlotUI target = null;
        string key = "";

        // 어떤 슬롯인지 판단
        switch (data.type)
        {
            case SkillsType.Active:
                if (slotZ.IsEmpty()) { target = slotZ; key = "Z"; }
                else if (slotX.IsEmpty()) { target = slotX; key = "X"; }
                break;

            case SkillsType.Ultimate:
                if (slotC.IsEmpty()) { target = slotC; key = "C"; }
                break;
        }

        if (target == null)
        {
            Debug.Log("HUD 슬롯이 비어있지 않습니다");
            return;
        }

        // HUD 슬롯 UI에 아이콘 / 쿨타임 / 키 지정
        target.SetSkill(data.icon, data.coolTime, key);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
