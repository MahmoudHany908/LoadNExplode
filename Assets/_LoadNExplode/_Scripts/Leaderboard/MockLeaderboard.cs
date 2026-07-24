using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

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
                PlayerName = i == 1 ? "FLUX" : $"Player_{Random.Range(1, 100)}",
                Score = Random.Range(10, 1000)
            });
        }

        fakeScores = fakeScores.OrderByDescending(entry => entry.Score).ToList();

        for (int i = 0; i < fakeScores.Count; i++)
        {
            var entry = fakeScores[i];
            entry.Rank = i + 1;
            fakeScores[i] = entry;
        }

        Debug.Log("[MOCK] Fetched and sorted top scores!");
        return fakeScores;
    }
}