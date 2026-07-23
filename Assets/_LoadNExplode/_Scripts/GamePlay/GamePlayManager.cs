using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GamePlayManager : MonoBehaviour
{
    [FormerlySerializedAs("deathUIPrefab")]
    [SerializeField] private GameObject spawnMapUI;

    private Player deadPlayer;

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
        EventBus.Subscribe<OnRestartGameButtonPressedEvent>(OnRestartGameButtonPressed);
        EventBus.Subscribe<RequestSpawnEvent>(OnSpawnRequested);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerDeathEvent>(OnPlayerDeath);
        EventBus.Unsubscribe<OnRestartGameButtonPressedEvent>(OnRestartGameButtonPressed);
        EventBus.Unsubscribe<RequestSpawnEvent>(OnSpawnRequested);

    }

    private void OnRestartGameButtonPressed(OnRestartGameButtonPressedEvent evt)
    {
        Time.timeScale = 1;
        string activeSceneName = SceneManager.GetActiveScene().name;
        EventBus.Publish(new RequestSceneLoadEvent(activeSceneName));

    }

    private void OnPlayerDeath(PlayerDeathEvent evt)
    {
        deadPlayer = evt.Player;
        Time.timeScale = 0f;

        if (spawnMapUI != null)
            spawnMapUI.SetActive(true);
    }

    private void OnSpawnRequested(RequestSpawnEvent evt)
    {
        if (deadPlayer == null || evt.SpawnPoint == null)
            return;

        deadPlayer.Respawn(evt.SpawnPoint.position);

        if (spawnMapUI != null)
            spawnMapUI.SetActive(false);

        Time.timeScale = 1f;
        EventBus.Publish(new PlayerSpawnedEvent(deadPlayer, evt.SpawnPoint));
        deadPlayer = null;
    }
}
