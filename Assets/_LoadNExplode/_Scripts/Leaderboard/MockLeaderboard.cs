using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MockLeaderboard : ILeaderboardService
{
    public async Task SubmitScoreAsync(string playerName, int score)
    {
        await Task.Delay(500);
        Debug.Log($"[MOCK] Score of {score} submitted for {playerName}!");
    }

    public async Task<List<LeaderboardEntry>> GetTopScoresAsync(int limit)
    {
        await Task.Delay(500);

        List<LeaderboardEntry> fakeScores = new List<LeaderboardEntry>();
        for (int i = 1; i <= limit; i++)
        {
            fakeScores.Add(new LeaderboardEntry
            {
                Rank = i,
                // Just generating some fake names for testing
                PlayerName = i == 1 ? "OZOZ" : $"Player_{Random.Range(1000, 9999)}",
                Score = Random.Range(1, 100)
            });
        }

        Debug.Log("[MOCK] Fetched top scores!");
        return fakeScores;
    }
}