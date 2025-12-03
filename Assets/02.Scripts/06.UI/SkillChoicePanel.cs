using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillChoicePanel : MonoBehaviour
{
    public static SkillChoicePanel Instance;

    [Header("Card")]
    public SkillCardUI cardPrefab;
    public Transform cardParent;

    private SkillOptionData[] currentOptions;

    [Header("Owned Item Slots")]
    public TItemSlotUI slotPrefab;

    public Transform weaponSlotParent;   // OwnedWeaponGroup
    public Transform equipSlotParent;    // OwnedEquipGroup

    [Header("캐릭터 스탯")]
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

    [Header("스킬 슬롯")]
    public SkillChoiceSlotUI slotZ;
    public SkillChoiceSlotUI slotX;
    public SkillChoiceSlotUI slotC;
    
    [Header("그 외")]
    public GameObject window;
    public bool isOpen { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        window.SetActive(false);
    }

    // 패널 열기
    public void Open(SkillOptionData[] options)
    {
        isOpen = true;

        window.SetActive(true);
        Time.timeScale = 0f;

        CreateCards(options);

        slotZ.SetEmpty();
        slotX.SetEmpty();
        slotC.SetEmpty();

        RefreshStatsDummy();
        CreateEmptyWeaponSlots();
        CreateEmptyEquipSlots();
    }

    // 카드 생성
    private void CreateCards(SkillOptionData[] options)
    {
        foreach (Transform child in cardParent)
            Destroy(child.gameObject);

        foreach (var opt in options)
        {
            var card = Instantiate(cardPrefab, cardParent);
            card.SetCard(opt);
        }
    }

    // 카드 선택
    public void SelectCard(SkillOptionData option)
    {
        // 궁극기 스킬이면 무조건 C 슬롯
        if (option.skillType == SkillType.Ultimate)
        {
            if (slotC.IsEmpty())
            {
                slotC.SetSkill(option);
            }
            else
            {
                Debug.Log("궁극기 슬롯이 이미 찼습니다!");
            }

            ClosePanel();
            return;
        }

        // 일반 스킬이면 Z → X 순서
        if (slotZ.IsEmpty())
        {
            slotZ.SetSkill(option);
        }
        else if (slotX.IsEmpty())
        {
            slotX.SetSkill(option);
        }
        else
        {
            Debug.Log("일반 스킬 슬롯 두 개가 이미 찼습니다!");
        }

        ClosePanel();
    }


    private void ClosePanel()
    {
        isOpen = false;

        window.SetActive(false);
        Time.timeScale = 1f;
    }

    // 임시 옵션 생성 (테스트용)
    private SkillOptionData[] CreateDummyOptions()
    {
        SkillOptionData[] arr = new SkillOptionData[2];

        for (int i = 0; i < 2; i++)
        {
            arr[i] = new SkillOptionData()
            {
                name = $"임시 옵션 {i + 1}",
                description = "테스트 설명",
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
        SkillOptionData[] dummy = CreateDummyOptions();
        Open(dummy);
    }
}
