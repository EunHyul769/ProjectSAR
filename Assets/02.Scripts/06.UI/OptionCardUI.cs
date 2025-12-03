using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OptionCardUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI")]
    public Image itemImage;
    public TMP_Text nameText;
    public TMP_Text levelText;
    public TMP_Text typeText;
    public TMP_Text optionText;
    public Image rarityFrame;

    [Header("Scale Root")]
    public Transform scaleRoot;

    private LevelUpOptionData optionData;
    private Vector3 originalScale;

    private void Start()
    {
        if (scaleRoot != null)
            originalScale = scaleRoot.localScale;
    }

    public void SetCard(LevelUpOptionData data)
    {
        optionData = data;

        if (data == null)
        {
            nameText.text = "데이터 없음";
            levelText.text = "-";
            typeText.text = "-";
            optionText.text = "데이터 준비중";
            rarityFrame.color = Color.gray;
            itemImage.sprite = null;
            return;
        }

        itemImage.sprite = data.icon;
        nameText.text = data.name;

        // 임시데이터
        levelText.text = "Lv2 -> Lv3";

        typeText.text = data.metaInfo;
        optionText.text = data.description;

        rarityFrame.color = data.rarityColor;
    }

    // 마우스 올리면 확대
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scaleRoot != null)
            scaleRoot.localScale = originalScale * 1.2f;
    }

    // 마우스가 빠지면 원래 크기
    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleRoot != null)
            scaleRoot.localScale = originalScale;
    }

    // 클릭 시 선택 처리
    public void OnPointerClick(PointerEventData eventData)
    {
        LevelUpPanel.Instance.SelectCard(optionData);
    }
}
