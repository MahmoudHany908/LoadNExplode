using System.Collections.Generic;
using System.Threading.Tasks;

public struct LeaderboardEntry
{
    public int Rank;
    public string PlayerName;
    public int Score;
}


public interface ILeaderboardService
{
    Task SubmitScoreAsync(string playerName, int score);
    Task<List<LeaderboardEntry>> GetTopScoresAsync(int limit);
}