using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Personal Best")]
    public TextMeshProUGUI personalBestText;

    [Header("Top 100 List")]
    public GameObject scoreEntryPrefab;
    public Transform scrollContentArea;

    private ILeaderboardService _leaderboardService;

    private void Start()
    {
        //_leaderboardService = new MockLeaderboard();
        _leaderboardService = new LiveLeaderboard();
        personalBestText.text = $"Highest Score: {PlayerPrefs.GetInt("PersonalBest", 0)}";
    }

    public async void LoadTop100()
    {
        foreach (Transform child in scrollContentArea)
        {
            Destroy(child.gameObject);
        }

        List<LeaderboardEntry> topScores = await _leaderboardService.GetTopScoresAsync(100);
        foreach (var entry in topScores)
        {
            GameObject newUIEntry = Instantiate(scoreEntryPrefab, scrollContentArea);
            newUIEntry.GetComponent<ScoreEntryUI>().Setup(entry.Rank, entry.PlayerName, entry.Score);
        }
    }
}