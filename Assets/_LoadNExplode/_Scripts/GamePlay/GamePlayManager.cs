using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GamePlayManager : MonoBehaviour
{
    [FormerlySerializedAs("deathUIPrefab")]
    [SerializeField] private GameObject mapUI;
    [SerializeField] private GameObject shopUI;
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
    }

    private void OnRestartGameButtonPressed(OnRestartGameButtonPressedEvent evt)
    {
        Time.timeScale = 1;
        string activeSceneName = SceneManager.GetActiveScene().name;
        EventBus.Publish(new RequestSceneLoadEvent(activeSceneName));

    }

    private void OnPlayerDeath(PlayerDeathEvent evt)
    {
        //do explosion effect here
        //then wait for a few seconds and then show the spawn map UI

        _palyer = evt.Player;
        _palyer.GetPlayerMovement().enabled = false;
        _palyer.GetPlayerVisuals().enabled = false;

        if (mapUI != null) mapUI.SetActive(true);
        if (shopUI != null) shopUI.SetActive(true);
    }

    private void OnSpawnRequested(RequestSpawnEvent evt)
    {
        if (_palyer == null || evt.SpawnPoint == null) return;

        Time.timeScale = 1f;

        if (mapUI != null) mapUI.SetActive(false);
        if (shopUI != null) shopUI.SetActive(false);

        _palyer.GetPlayerMovement().enabled = true;
        _palyer.GetPlayerVisuals().enabled = true;

        _palyer.Respawn(evt.SpawnPoint.position);


        EventBus.Publish(new PlayerSpawnedEvent(_palyer, evt.SpawnPoint));

        _palyer = null;
    }
}
