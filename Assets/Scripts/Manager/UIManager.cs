using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD")]
    public Slider hpBar;
    public Slider expBar;
    public TMP_Text timerText;

    [Header("Panels")]
    public LevelUpPanel levelUpPanel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateTimer(float time)
    {
        int min = (int)(time / 60);
        int sec = (int)(time % 60);
        timerText.text = $"{min:00}:{sec:00}";
    }

    // 레벨업 패널 열기->나중에 GameManager가 호출하게 됨
    public void OpenLevelUp(LevelUpOptionData[] options)
    {
        levelUpPanel.Open(options);
    }
}
