using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TItemSlotUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TMP_Text levelText;
    public Sprite emptySprite;

    public void SetEmpty()
    {
        icon.enabled = true;            // ≤Ù¡ˆ æ ¿Ω
        icon.sprite = emptySprite;      // empty ΩΩ∑‘ ¿ÃπÃ¡ˆ
        levelText.text = "Lv -";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ENTER SLOT, Instance = " + ItemTooltipUI.Instance);

        if (ItemTooltipUI.Instance == null)
            return;

        ItemTooltipUI.Instance.ShowEmpty();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer EXIT on SLOT");

        if (ItemTooltipUI.Instance != null)
            ItemTooltipUI.Instance.Hide();
    }
}
