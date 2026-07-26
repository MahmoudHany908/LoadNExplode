using UnityEngine;


public class BarrelRainRunner : MonoBehaviour
{
    private GameObject barrelPrefab;
    private int totalBarrels;
    private float duration;
    private float spawnHeight;
    private float spawnRadius;
    private Transform originTransform;

    private float timeElapsed;
    private int barrelsSpawned;

    public void Init(GameObject barrelPrefab, int totalBarrels, float duration, float spawnHeight, float spawnRadius, Transform originTransform)
    {
        this.barrelPrefab = barrelPrefab;
        this.totalBarrels = totalBarrels;
        this.duration = duration;
        this.spawnHeight = spawnHeight;
        this.spawnRadius = spawnRadius;
        this.originTransform = originTransform;

        timeElapsed = 0f;
        barrelsSpawned = 0;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        float progress = Mathf.Clamp01(timeElapsed / duration);
        int targetBarrelsSpawned = Mathf.FloorToInt(progress * totalBarrels);

        while (barrelsSpawned < targetBarrelsSpawned && barrelsSpawned < totalBarrels)
        {
            SpawnBarrel();
            barrelsSpawned++;
        }

        if (timeElapsed >= duration && barrelsSpawned >= totalBarrels)
        {
            Debug.Log($"[{gameObject.name}] BarrelRainRunner finished — spawned {barrelsSpawned} barrels.");
            Destroy(gameObject);
        }
    }

    private void SpawnBarrel()
    {
        if (barrelPrefab == null || originTransform == null)
        {
            return;
        }

        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);
        Vector3 spawnPos = originTransform.position + randomOffset + Vector3.up * spawnHeight;

        GameObject barrelInstance = Instantiate(barrelPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = barrelInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.down * 2f, ForceMode.VelocityChange);
        }
    }
}