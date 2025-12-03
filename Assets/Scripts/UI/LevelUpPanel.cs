using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
    public static LevelUpPanel Instance;

    [Header("Card")]
    public OptionCardUI cardPrefab;
    public Transform cardParent;

    [Header("Buttons")]
    public Button rerollButton;
    public Button deleteButton;   // Delete 버튼 (UI만 존재)

    private LevelUpOptionData[] currentOptions;

    [Header("Owned Item Slots")]
    public TItemSlotUI slotPrefab;

    public Transform weaponSlotParent;   // OwnedWeaponGroup
    public Transform equipSlotParent;    // OwnedEquipGroup

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

    public bool isOpen { get; private set; }
    public GameObject window;

    private void Awake()
    {
        Instance = this;

        //gameObject.SetActive(false);

        rerollButton.interactable = false;

        // Delete 버튼은 이번 프로젝트에서 비활성화
        deleteButton.interactable = false;
    }

    private void Start()
    {
        // TestOpen();
        window.SetActive(false);
    }

    // 패널 열기
    public void Open(LevelUpOptionData[] options)
    {
        isOpen = true;

        window.SetActive(true);
        Time.timeScale = 0f;

        CreateCards(options);
        RefreshStatsDummy();
        CreateEmptyWeaponSlots();
        CreateEmptyEquipSlots();
    }

    // 카드 생성
    private void CreateCards(LevelUpOptionData[] options)
    {
        foreach (Transform child in cardParent)
            Destroy(child.gameObject);

        currentOptions = options;

        foreach (var opt in options)
        {
            var card = Instantiate(cardPrefab, cardParent);
            card.SetCard(opt);
        }
    }

    // 카드 선택
    public void SelectCard(LevelUpOptionData option)
    {
        Debug.Log("선택됨: " + option.name);

        // GameManager가 준비되면 아래 줄 활성화
        // GameManager.Instance.ApplyLevelUp(option);

        ClosePanel();
    }

    private void ClosePanel()
    {
        isOpen = false;

        window.SetActive(false);
        Time.timeScale = 1f;
    }

    // 임시 옵션 생성 (테스트용)
    private LevelUpOptionData[] CreateDummyOptions()
    {
        LevelUpOptionData[] arr = new LevelUpOptionData[3];

        for (int i = 0; i < 3; i++)
        {
            arr[i] = new LevelUpOptionData()
            {
                name = $"임시 옵션 {i + 1}",
                metaInfo = "희귀도/타입/보유수",
                description = "테스트 설명",
                rarityColor = Color.white
            };
        }

        return arr;
    }

    private void RefreshStatsDummy()
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
    private void CreateEmptyWeaponSlots()
    {
        foreach (Transform child in weaponSlotParent)
            Destroy(child.gameObject);

        for (int i = 0; i < 6; i++)
        {
            var slot = Instantiate(slotPrefab, weaponSlotParent);
            slot.SetEmpty();
        }
    }

    private void CreateEmptyEquipSlots()
    {
        foreach (Transform child in equipSlotParent)
            Destroy(child.gameObject);

        for (int i = 0; i < 6; i++)
        {
            var slot = Instantiate(slotPrefab, equipSlotParent);
            slot.SetEmpty();
        }
    }

    // 테스트 함수
    public void TestOpen()
    {
        LevelUpOptionData[] dummy = CreateDummyOptions();
        Open(dummy);
    }
}
