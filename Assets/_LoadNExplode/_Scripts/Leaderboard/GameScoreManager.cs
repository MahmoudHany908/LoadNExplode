using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    public static GameScoreManager Instance { get; private set; }

    public int CurrentKills { get; private set; }
    public int LocalHighScore { get; private set; }

    private ILeaderboardService _leaderboardService;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }

        LocalHighScore = PlayerPrefs.GetInt("PersonalBest", 0);


        //_leaderboardService = new MockLeaderboard();
        _leaderboardService = new LiveLeaderboard(); ;
    }

    public void AddKill()
    {
        CurrentKills++;
    }

    public async void EndRun(string playerName)
    {
        if (CurrentKills > LocalHighScore)
        {
            LocalHighScore = CurrentKills;
            PlayerPrefs.SetInt("PersonalBest", LocalHighScore);
            PlayerPrefs.Save();
            await _leaderboardService.SubmitScoreAsync(playerName, LocalHighScore);
        }

        CurrentKills = 0; 
    }
}