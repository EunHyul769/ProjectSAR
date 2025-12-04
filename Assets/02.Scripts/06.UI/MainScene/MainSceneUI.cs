using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSceneUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button btnStart;
    public Button btnUpgrade;
    public Button btnBook;
    public Button btnSettings;
    public Button btnQuit;

    [Header("setting")]
    public GameObject settingsPopup;

    private void Start()
    {
        btnStart.onClick.AddListener(OnClickStart);
        btnUpgrade.onClick.AddListener(OnClickUpgrade);
        btnBook.onClick.AddListener(OnClickBook);
        btnSettings.onClick.AddListener(OnClickSettings);
        btnQuit.onClick.AddListener(OnClickQuit);
    }

    private void OnClickStart()
    {
        SceneLoader.Load(SceneType.CharacterSelectScene);
    }

    private void OnClickUpgrade()
    {
        Debug.Log("���׷��̵�(������) - ���� ��� ����");
    }

    private void OnClickBook()
    {
        Debug.Log("����(������) - ���� ��� ����");
    }

    private void OnClickSettings()
    {
        settingsPopup.SetActive(true);
    }

    private void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("���� ����");
    }
}
