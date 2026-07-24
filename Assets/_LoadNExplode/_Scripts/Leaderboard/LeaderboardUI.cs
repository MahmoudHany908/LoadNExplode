using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{


    [Header("Top 100 List")]
    public GameObject scoreEntryPrefab;
    public Transform scrollContentArea;

    private ILeaderboardService _leaderboardService;

    private void Awake()
    {
        _leaderboardService = new LiveLeaderboard();
    }

    private void OnEnable()
    {
        RefreshLeaderboard();
    }

    public async void RefreshLeaderboard()
    {
        // Clear any currently instantiated UI rows
        foreach (Transform child in scrollContentArea)
        {
            Destroy(child.gameObject);
        }

        // Re-fetch the updated scores
        List<LeaderboardEntry> topScores = await _leaderboardService.GetTopScoresAsync(100);
        foreach (var entry in topScores)
        {
            GameObject newUIEntry = Instantiate(scoreEntryPrefab, scrollContentArea);
            newUIEntry.GetComponent<ScoreEntryUI>().Setup(entry.Rank, entry.PlayerName, entry.Score);
        }
    }

    // Fallback in case your 'Load Top 100' button is still pointing to this method name in the Inspector
    public void LoadTop100()
    {
        RefreshLeaderboard();
    }
}