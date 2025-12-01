using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Main,
    CharacterSelect,
    StageSelect,
    Game,
    Result
}

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType<SceneLoader>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    public static void Load(SceneType type)
    {
        SceneManager.LoadScene(type.ToString());
    }
}

