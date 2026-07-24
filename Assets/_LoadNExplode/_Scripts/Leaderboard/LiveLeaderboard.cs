using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using LootLocker.Requests;

public class LiveLeaderboard : ILeaderboardService
{
    private readonly string leaderboardID = "35697";

    public async Task SubmitScoreAsync(string playerName, int score)
    {
        TaskCompletionSource<bool> tcs = new();

        LootLockerSDKManager.SubmitScore("", score, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score!");
            }
            tcs.SetResult(response.success);
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
                if (response.items != null)
                {
                    foreach (var member in response.items)
                    {
                        string pName = "Unknown";
                        if (member.player != null && !string.IsNullOrEmpty(member.player.name))
                        {
                            pName = member.player.name;
                        }

                        formattedScores.Add(new LeaderboardEntry
                        {
                            Rank = member.rank,
                            PlayerName = pName,
                            Score = member.score
                        });
                    }
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