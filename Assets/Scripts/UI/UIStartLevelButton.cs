using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class UIStartLevelButton : MonoBehaviour
{
    [SerializeField] string _levelName;
    [SerializeField] int _levelID;

    public string LevelName => _levelName;

    public int LevelID => _levelID;

    public void LoadLevel(int levelToLoad)
    {
        // SceneManager.LoadScene(_levelName);
        SceneManager.LoadScene(levelToLoad);
        ScoreSystem.ResetScore();
    }
}
