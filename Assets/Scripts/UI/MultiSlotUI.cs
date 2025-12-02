using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MultiSlotUI : MonoBehaviour
{
    [Header("Slot Type")]
    //public SlotType slotType; // Skill / Buff / Debuff
    //SlotType 구현 전으로 주석처리(데이터 추가되면 연결)

    [Header("UI")]
    public Image icon;
    public Image cooldownMask;
    public TMP_Text keyText;        // Skill 전용
    public TMP_Text cooldownText;   // 공통 (Cooldown / Duration)

    // 공통 쿨다운 / 지속시간
    private float currentTime;
    private float maxTime;

    public void SetSkill(Sprite iconSprite, float cooldown, string key)
    {
        //slotType = SlotType.Skill;

        icon.sprite = iconSprite;
        icon.enabled = true;

        maxTime = cooldown;
        currentTime = 0f;

        keyText.text = key;
        keyText.gameObject.SetActive(true);  // Skill에만 표시

        cooldownMask.fillAmount = 0f;
        cooldownText.gameObject.SetActive(false);
    }

    public void SetBuff(Sprite iconSprite, float duration)
    {
        //slotType = SlotType.Buff;

        icon.sprite = iconSprite;
        icon.enabled = true;

        maxTime = duration;
        currentTime = duration;

        keyText.gameObject.SetActive(false); // Buff는 키 표시 없음

        cooldownMask.fillAmount = 1f;
        cooldownText.text = Mathf.Ceil(duration).ToString();
        cooldownText.gameObject.SetActive(true);

        StartCoroutine(CountDownRoutine());
    }

    public void SetDebuff(Sprite iconSprite, float duration)
    {
        //slotType = SlotType.Debuff;

        icon.sprite = iconSprite;
        icon.enabled = true;

        maxTime = duration;
        currentTime = duration;

        keyText.gameObject.SetActive(false);

        cooldownMask.fillAmount = 1f;
        cooldownText.text = Mathf.Ceil(duration).ToString();
        cooldownText.gameObject.SetActive(true);

        StartCoroutine(CountDownRoutine());
    }

    public void StartCooldown()
    {
        //if (slotType != SlotType.Skill) return;

        //currentTime = maxTime;
        //StartCoroutine(CountDownRoutine());
    }

    private IEnumerator CountDownRoutine()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            float ratio = currentTime / maxTime;

            cooldownMask.fillAmount = ratio;
            cooldownText.text = Mathf.Ceil(currentTime).ToString();
            cooldownText.gameObject.SetActive(true);

            yield return null;
        }

        cooldownMask.fillAmount = 0;
        cooldownText.gameObject.SetActive(false);
    }
}
