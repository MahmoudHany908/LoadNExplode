using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GamePlayManager : MonoBehaviour
{
    [FormerlySerializedAs("deathUIPrefab")]
    [SerializeField] private GameObject DeathPanelUI;

    [SerializeField] private GameObject runEndUI;

    private Player _palyer;

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
        EventBus.Subscribe<RequestSpawnEvent>(OnSpawnRequested);

        EventBus.Subscribe<OnCountdownFinishedEvent>(OnCountdownFinished);
        EventBus.Subscribe<OnRestartGameButtonPressedEvent>(OnRestartGameButtonPressed);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerDeathEvent>(OnPlayerDeath);
        EventBus.Unsubscribe<RequestSpawnEvent>(OnSpawnRequested);


        EventBus.Unsubscribe<OnRestartGameButtonPressedEvent>(OnRestartGameButtonPressed);

    }


    private void OnCountdownFinished(OnCountdownFinishedEvent evt)
    {
        Time.timeScale = 0;
        
        if (runEndUI != null)
        {
            Instantiate(runEndUI);
        }
        else
        {
            Debug.LogWarning("runEndUI is not assigned on GamePlayManager.", this);
        }

        if (GameScoreManager.Instance != null)
        {
            int currentKills = GameScoreManager.Instance.CurrentKills;
            string savedName = PlayerPrefs.GetString("SavedPlayerName", "Unknown");
            GameScoreManager.Instance.EndRun(savedName);
        }
        else
        {
            Debug.LogError("GameScoreManager.Instance is null! This usually happens if you play the GamePlay scene directly in the Editor without passing through the MainMenu scene where it is created.", this);
        }
    }

    private void OnRestartGameButtonPressed(OnRestartGameButtonPressedEvent evt)
    {
        Time.timeScale = 1;
        string activeSceneName = SceneManager.GetActiveScene().name;
        EventBus.Publish(new RequestSceneLoadEvent(activeSceneName));

    }

    private void OnPlayerDeath(PlayerDeathEvent evt)
    {
        _palyer = evt.Player;
        StartCoroutine(ToggleDeathPanel(true, 1.5f));

    }

    private void OnSpawnRequested(RequestSpawnEvent evt)
    {
        if (_palyer == null || evt.SpawnPoint == null) return;

        _palyer.Respawn(evt.SpawnPoint.position);
        StartCoroutine(ToggleDeathPanel(false, 0.1f));

        EventBus.Publish(new PlayerSpawnedEvent(_palyer, evt.SpawnPoint));
        _palyer = null;
    }

    private IEnumerator ToggleDeathPanel(bool isActive, float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 1f;
        if (DeathPanelUI != null) DeathPanelUI.SetActive(isActive);
    }
}
