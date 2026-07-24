using UnityEngine;

public class GoldDropper : MonoBehaviour
{
    [SerializeField] private GoldPickup goldPickupPrefab;
    [SerializeField] private GoldManager goldManager;

    private void Awake()
    {
        if (goldManager == null)
            goldManager = FindFirstObjectByType<GoldManager>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<EnemyDeathEvent>(OnEnemyDeath);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<EnemyDeathEvent>(OnEnemyDeath);
    }

    private void OnEnemyDeath(EnemyDeathEvent evt)
    {
        if (evt.GoldReward <= 0)
            return;

        if (goldPickupPrefab == null)
        {
            if (goldManager != null)
                goldManager.AddGold(evt.GoldReward);

            return;
        }

        GoldPickup pickup = Instantiate(goldPickupPrefab, evt.DeathPosition, Quaternion.identity);
        pickup.SetAmount(evt.GoldReward);
    }
}
