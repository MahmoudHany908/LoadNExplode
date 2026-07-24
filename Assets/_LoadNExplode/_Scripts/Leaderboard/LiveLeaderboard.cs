using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using LootLocker.Requests;

public class LiveLeaderboard : ILeaderboardService
{
    private readonly string leaderboardID = "35697";

    public async Task SubmitScoreAsync(string playerName, int score)
    {
        await SetPlayerNameAsync(playerName);

        TaskCompletionSource<bool> tcs = new();

        LootLockerSDKManager.SubmitScore("", score, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score!");
                tcs.SetResult(true);
            }
            else
            {
                Debug.LogError("Failed to upload score: " + response.errorData.message);
                tcs.SetResult(false);
            }
        });

        await tcs.Task;
    }

    public async Task<List<LeaderboardEntry>> GetTopScoresAsync(int limit)
    {
        TaskCompletionSource<List<LeaderboardEntry>> tcs = new();

        LootLockerSDKManager.GetScoreList(leaderboardID, limit, (response) =>
        {
            List<LeaderboardEntry> formattedScores = new();

            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;
                foreach (var member in members)
                {
                    formattedScores.Add(new()
                    {
                        Rank = member.rank,
                        PlayerName = string.IsNullOrEmpty(member.player.name) ? "Unknown" : member.player.name,
                        Score = member.score
                    });
                }
                tcs.SetResult(formattedScores);
            }
            else
            {
                Debug.LogError("Failed to fetch scores: " + response.errorData.message);
                tcs.SetResult(formattedScores);
            }
        });

        return await tcs.Task;
    }

    private Task<bool> SetPlayerNameAsync(string name)
    {
        TaskCompletionSource<bool> tcs = new();
        LootLockerSDKManager.SetPlayerName(name, (response) =>
        {
            if (response.success) tcs.SetResult(true);
            else tcs.SetResult(false);
        });
        return tcs.Task;
    }
}