using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [Header("Skill Select Slots")]
    public int skillSlotCount = 15;
    public Transform skillSlotParent; 
    public GameObject skillSlotPrefab;  

    [Header("Owned Item Slots")]
    public int itemSlotCount = 48;
    public Transform itemSlotParent;
    public GameObject itemSlotPrefab; 

    [Header("Character Stats")]
    public TMP_Text hpText;
    public TMP_Text hpgenText;
    public TMP_Text defText;
    public TMP_Text spdText;
    public TMP_Text atkText;
    public TMP_Text atkspdText;
    public TMP_Text atkareaText;
    public TMP_Text cri;
    public TMP_Text cridmg;
    public TMP_Text projectilespd;
    public TMP_Text dur;
    public TMP_Text cd;
    public TMP_Text projectilenum;

    [Header("Buttons")]
    public Button resumeButton;   // 이어하기
    public Button retryButton;    // 재시작
    public Button mainButton;     // 메인 화면

    [Header("OpenClose")]
    public GameObject window;
    public bool isOpen = false;

    private void Awake()
    {
        // 버튼 기능 연결
        resumeButton.onClick.AddListener(OnResumeClicked);
        retryButton.onClick.AddListener(OnRetryClicked);
        mainButton.onClick.AddListener(OnMainClicked);
    }

    private void Start()
    {
        GenerateSkillSlots();
        GenerateItemSlots();
        RefreshStatsDummy();
        window.SetActive(false);   // PausePanel은 기본 비활성화
    }


    //자동 슬롯 생성
    void GenerateSkillSlots()
    {
        foreach (Transform child in skillSlotParent)
            Destroy(child.gameObject);

        for (int i = 0; i < skillSlotCount; i++)
        {
            Instantiate(skillSlotPrefab, skillSlotParent);
        }
    }

    void GenerateItemSlots()
    {
        foreach (Transform child in itemSlotParent)
            Destroy(child.gameObject);

        for (int i = 0; i < itemSlotCount; i++)
        {
            Instantiate(itemSlotPrefab, itemSlotParent);
        }
    }

    //스탯 표시 더미 (LevelUpPanel 참고)
    public void RefreshStatsDummy()
    {
        hpText.text = "-";
        hpgenText.text = "-";
        defText.text = "-";
        spdText.text = "-";
        atkText.text = "-";
        atkspdText.text = "-";
        atkareaText.text = "-";
        cri.text = "-";
        cridmg.text = "-";
        projectilespd.text = "-";
        dur.text = "-";
        cd.text = "-";
        projectilenum.text = "-";
    }

    //버튼 기능
    void OnResumeClicked()
    {
        Close();
    }

    void OnRetryClicked()
    {
        // 패널 먼저 닫기
        window.SetActive(false);
        isOpen = false;

        // 타임스케일 복구
        Time.timeScale = 1f;

        // 게임씬 다시 로드
        SceneLoader.Load(SceneType.GameScene);
    }

    void OnMainClicked()
    {
        // 메인 화면으로 이동
        SceneLoader.Load(SceneType.MainScene);
    }

    //외부에서 ESC로 열기

    public void Open()
    {
        if (isOpen) return;

        window.SetActive(true);
        Time.timeScale = 0f;
        RefreshStatsDummy(); // 나중엔 실제 플레이어 스탯 불러오면 됨

        isOpen = true;
    }
    public void Close()
    {
        window.SetActive(false);
        Time.timeScale = 1f;

        isOpen = false;
    }
}
