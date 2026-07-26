using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    public static GameScoreManager Instance { get; private set; }

    public int CurrentKills { get; private set; }
    public int LocalHighScore { get; private set; }
    public float CurrentPanicLevel { get; private set; } = 1f;

    private ILeaderboardService _leaderboardService;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }

        LocalHighScore = PlayerPrefs.GetInt("PersonalBest", 0);

       
        //_leaderboardService = new MockLeaderboard();
        _leaderboardService = new LiveLeaderboard();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<EnemyDeathEvent>(HandleEnemyKilled);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<EnemyDeathEvent>(HandleEnemyKilled);
    }


    private void HandleEnemyKilled(EnemyDeathEvent deathEvent)
    {
        AddKill();
        CurrentPanicLevel += 1f;
        Debug.Log($"Enemy Killed: {deathEvent.BehaviorType} at {deathEvent.DeathPosition}. Total Kills: {CurrentKills}, New Panic Level: {CurrentPanicLevel}");
    }

    public void AddKill()
    {
        CurrentKills++;
    }

    public async void EndRun(string playerName)
    {
        await _leaderboardService.SubmitScoreAsync(playerName, CurrentKills);

        if (CurrentKills > LocalHighScore)
        {
            LocalHighScore = CurrentKills;
            PlayerPrefs.SetInt("PersonalBest", LocalHighScore);
            PlayerPrefs.Save();
        }

        CurrentKills = 0;
    }
}