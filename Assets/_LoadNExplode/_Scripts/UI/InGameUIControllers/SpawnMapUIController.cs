using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpawnMapUIController : MonoBehaviour
{
    [SerializeField] private List<Button> spawnButtons = new();
    [SerializeField] private List<Transform> spawnPoints = new();

    private readonly List<UnityAction> _buttonListeners = new();

    private void OnEnable()
    {
        int count = Mathf.Min(spawnButtons.Count, spawnPoints.Count);

        for (int i = 0; i < count; i++)
        {
            Button button = spawnButtons[i];
            Transform spawnPoint = spawnPoints[i];

            if (button == null || spawnPoint == null)
                continue;

            UnityAction listener = () => RequestSpawn(spawnPoint);
            _buttonListeners.Add(listener);
            button.onClick.AddListener(listener);
        }
    }

    private void OnDisable()
    {
        int count = Mathf.Min(spawnButtons.Count, _buttonListeners.Count);

        for (int i = 0; i < count; i++)
        {
            if (spawnButtons[i] != null)
                spawnButtons[i].onClick.RemoveListener(_buttonListeners[i]);
        }

        _buttonListeners.Clear();
    }

    private void RequestSpawn(Transform spawnPoint)
    {
        EventBus.Publish(new RequestSpawnEvent(spawnPoint));
    }
}
