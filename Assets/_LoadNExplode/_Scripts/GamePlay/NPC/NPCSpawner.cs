using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private NPCBehaviorType spawnerType;
    [SerializeField] private int baseMaxCapacity = 10;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnRadius = 40f;

    private List<GameObject> activeNPCs = new List<GameObject>();

    private void OnEnable()
    {
        EventBus.Subscribe<EnemyDeathEvent>(HandleEnemyDeath);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<EnemyDeathEvent>(HandleEnemyDeath);
    }

    private void HandleEnemyDeath(EnemyDeathEvent evt)
    {
        if (activeNPCs.Contains(evt.EnemyGameObject))
        {
            activeNPCs.Remove(evt.EnemyGameObject);
        }
    }

    private int CalculateMaxCapacity()
    {
        if (spawnerType == NPCBehaviorType.Civilian)
        {
            return baseMaxCapacity;
        }
        else // Hostile
        {
            float panicLevel = 1f;
            if (GameScoreManager.Instance != null)
            {
                panicLevel = GameScoreManager.Instance.CurrentPanicLevel;
            }
            return baseMaxCapacity + Mathf.FloorToInt(panicLevel);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Clean up list in case NPCs were destroyed without EventBus
            activeNPCs.RemoveAll(npc => npc == null);

            if (activeNPCs.Count < CalculateMaxCapacity())
            {
                Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
                randomDirection += transform.position;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
                {
                    GameObject newNPC = Instantiate(npcPrefab, hit.position, Quaternion.identity);
                    activeNPCs.Add(newNPC);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
