using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    public Image skillIcon;
    public TMP_Text skillName;
    public TMP_Text skillDesc;
    public TMP_Text skillOption;

    public void SetSlot(SkillData data)
    {
        if (data == null)
        {
            // X 표시 or 비어있는 슬롯
            skillIcon.color = new Color(1, 1, 1, 0.2f);
            skillName.text = "";
            skillDesc.text = "";
            skillOption.text = "";
            return;
        }

        skillIcon.sprite = data.icon;
        skillName.text = data.skillName;
        skillDesc.text = data.description;

        // 옵션 표시 (원하면 자유롭게 수정 가능)
        skillOption.text =
            $"Type: {data.type}\n" +
            $"Cooldown: {data.coolTime}\n" +
            $"Value: {data.value1}\n" +
            $"Duration: {data.duration}";
    }
}
