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
    public Button resumeButton;   // �̾��ϱ�
    public Button retryButton;    // �����
    public Button mainButton;     // ���� ȭ��

    [Header("OpenClose")]
    public GameObject window;
    public bool isOpen = false;

    private void Awake()
    {
        // ��ư ��� ����
        resumeButton.onClick.AddListener(OnResumeClicked);
        retryButton.onClick.AddListener(OnRetryClicked);
        mainButton.onClick.AddListener(OnMainClicked);
    }

    private void Start()
    {
        GenerateSkillSlots();
        GenerateItemSlots();
        RefreshStatsDummy();
        window.SetActive(false);   // PausePanel�� �⺻ ��Ȱ��ȭ
    }


    //�ڵ� ���� ����
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

    //���� ǥ�� ���� (LevelUpPanel ����)
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

    //��ư ���
    void OnResumeClicked()
    {
        Close();
    }

    void OnRetryClicked()
    {
        // �г� ���� �ݱ�
        window.SetActive(false);
        isOpen = false;

        // Ÿ�ӽ����� ����
        Time.timeScale = 1f;

        // ���Ӿ� �ٽ� �ε�
        SceneLoader.Load(SceneType.GameScene);
    }

    void OnMainClicked()
    {
        // ���� ȭ������ �̵�
        SceneLoader.Load(SceneType.MainScene);
    }

    //�ܺο��� ESC�� ����

    public void Open()
    {
        if (isOpen) return;

        window.SetActive(true);
        Time.timeScale = 0f;
        RefreshStatsDummy(); // ���߿� ���� �÷��̾� ���� �ҷ����� ��

        isOpen = true;
    }
    public void Close()
    {
        window.SetActive(false);
        Time.timeScale = 1f;

        isOpen = false;
    }
}
