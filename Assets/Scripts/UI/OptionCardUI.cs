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
    public TMP_Text metaText;
    public TMP_Text optionText;
    public Image rarityFrame;

    private LevelUpOptionData optionData;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void SetCard(LevelUpOptionData data)
    {
        optionData = data;

        if (data == null)
        {
            nameText.text = "데이터 없음";
            metaText.text = "-";
            optionText.text = "데이터 준비중";
            rarityFrame.color = Color.gray;
            return;
        }

        itemImage.sprite = data.icon;
        nameText.text = data.name;
        metaText.text = data.metaInfo;
        optionText.text = data.description;
        rarityFrame.color = data.rarityColor;
    }

    // 마우스 올리면 20% 확대
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.2f;
    }

    // 마우스가 빠지면 원래 크기
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    // 클릭하면 선택 신호 보내기
    public void OnPointerClick(PointerEventData eventData)
    {
        LevelUpPanel.Instance.SelectCard(optionData);
    }
}
