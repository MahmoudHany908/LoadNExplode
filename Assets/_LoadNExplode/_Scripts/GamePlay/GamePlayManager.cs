using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GamePlayManager : MonoBehaviour
{
    [FormerlySerializedAs("deathUIPrefab")]
    [SerializeField] private GameObject spawnMapUI;
    [SerializeField] private GameObject shopUI;
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
        Instantiate(runEndUI);
        int currentKills = GameScoreManager.Instance.CurrentKills;
        string savedName = PlayerPrefs.GetString("SavedPlayerName", "Unknown");
        GameScoreManager.Instance.EndRun(savedName);
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

        if (spawnMapUI != null)
            spawnMapUI.SetActive(true);
        if (shopUI != null)
            shopUI.SetActive(true);
    }

    private void OnSpawnRequested(RequestSpawnEvent evt)
    {
        if (_palyer == null || evt.SpawnPoint == null) return;

        Time.timeScale = 1f;

        if (spawnMapUI != null) spawnMapUI.SetActive(false);
        if (shopUI != null) shopUI.SetActive(false);

        _palyer.GetPlayerMovement().enabled = true;
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
