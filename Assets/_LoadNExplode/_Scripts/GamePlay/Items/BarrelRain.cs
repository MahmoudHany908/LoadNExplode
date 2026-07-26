using UnityEngine;

public class BarrelRain : MonoBehaviour, IItem, IPlayerReceivable
{
    [Header("Barrel Rain Settings")]
    [Tooltip("The prefab of the barrel to spawn.")]
    [SerializeField] private GameObject barrelPrefab;
    [Tooltip("Total number of barrels to spawn (the 'x amount').")]
    [SerializeField] private int totalBarrels = 10;
    [Tooltip("Total time in seconds over which the barrels will spawn (the 'x sec').")]
    [SerializeField] private float duration = 2f;
    [Tooltip("How high above the player the barrels will spawn.")]
    [SerializeField] private float spawnHeight = 10f;
    [Tooltip("Random spread radius around the player. Prevents barrels from clipping into each other at the exact same coordinate.")]
    [SerializeField] private float spawnRadius = 2f;

    private Player player;
    private Transform playerTransform;

    public void SetPlayer(Player player)
    {
        this.player = player;
        this.playerTransform = player != null ? player.transform : null;

        if (this.playerTransform == null)
        {
            Debug.LogWarning($"[{gameObject.name}] BarrelRain received a null player reference.");
        }
    }

    public void Activate()
    {
        if (barrelPrefab == null)
        {
            Debug.LogWarning($"[{gameObject.name}] BarrelRain has no barrelPrefab assigned.");
            Destroy(gameObject);
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning($"[{gameObject.name}] BarrelRain cannot activate — player reference is null.");
            Destroy(gameObject);
            return;
        }

        if (totalBarrels <= 0)
        {
            Debug.LogWarning($"[{gameObject.name}] BarrelRain totalBarrels is <= 0. Nothing to spawn.");
            Destroy(gameObject);
            return;
        }

        float safeDuration = duration <= 0f ? 0.1f : duration;


        GameObject runnerObject = new GameObject("BarrelRainRunner");
        BarrelRainRunner runner = runnerObject.AddComponent<BarrelRainRunner>();
        runner.Init(barrelPrefab, totalBarrels, safeDuration, spawnHeight, spawnRadius, playerTransform);

        Debug.Log($"[{gameObject.name}] BarrelRain activated by {playerTransform.name}. Spawning {totalBarrels} barrels over {safeDuration} seconds.");

        Destroy(gameObject);
    }

    public void Started()
    {

    }

    public void Tick(float deltaTime)
    {

    }
}