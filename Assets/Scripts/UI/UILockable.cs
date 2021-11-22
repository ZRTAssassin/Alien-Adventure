using UnityEngine;

public class UILockable : MonoBehaviour
{
   
    void OnEnable()
    {
        var startButton = GetComponent<UIStartLevelButton>();
        string key = startButton.LevelName + "Unlocked"; // "Level1Unlocked"
        int unlocked = PlayerPrefs.GetInt(key, 0);
        if (unlocked == 0)
            gameObject.SetActive(false);
    }

    [ContextMenu("Clear Unlocked Level")]
    void ClearLevelUnlcoked()
    {
        var startButton = GetComponent<UIStartLevelButton>();
        string key = startButton.LevelName + "Unlocked"; // "Level1Unlocked"
        PlayerPrefs.DeleteKey(key);
    }
}