using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillChoiceSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;                        // 프리팹의 아이콘
    public SkillOptionData currentSkill;      // 슬롯에 들어간 스킬

    public SkillType slotType;                // Normal = Z/X, Ultimate = C

    public bool IsEmpty()
    {
        return currentSkill == null;
    }

    public void SetSkill(SkillOptionData data)
    {
        currentSkill = data;

        icon.sprite = data.icon;
        icon.enabled = true;
    }

    public void SetEmpty()
    {
        currentSkill = null;
        icon.enabled = false;
    }

    // 팝업 (TooltipUI 재사용)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemTooltipUI.Instance == null)
            return;

        if (currentSkill == null)
            ItemTooltipUI.Instance.ShowEmpty();
        else
            ItemTooltipUI.Instance.ShowSkill(currentSkill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemTooltipUI.Instance != null)
            ItemTooltipUI.Instance.Hide();
    }
}
