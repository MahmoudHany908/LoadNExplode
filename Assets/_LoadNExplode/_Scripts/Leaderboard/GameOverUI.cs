using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI bestScoreText;

    private void OnEnable()
    {
        int currentKills = GameScoreManager.Instance.CurrentKills;
        currentScoreText.text = $"Score: {currentKills}";
        string savedName = PlayerPrefs.GetString("SavedPlayerName", "Unknown");
        GameScoreManager.Instance.EndRun(savedName);

        bestScoreText.text = $"Personal Best: {PlayerPrefs.GetInt("PersonalBest", 0)}";
    }
}