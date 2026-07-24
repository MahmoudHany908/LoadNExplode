using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private int startingGold;
    [SerializeField] private GameObject goldPickupPrefab;

    public int Gold { get; private set; }

    private void Awake()
    {
        Gold = startingGold;
    }

    private void Start()
    {
        EventBus.Publish(new GoldChangedEvent(Gold));
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
        GameObject pickup = Instantiate(goldPickupPrefab, evt.DeathPosition, Quaternion.identity);
        pickup.GetComponent<GoldPickup>().SetAmount(evt.GoldReward, this);
    }



    public bool TrySpend(int amount)
    {
        if (amount < 0 || Gold < amount)
            return false;

        Gold -= amount;
        EventBus.Publish(new GoldChangedEvent(Gold));
        return true;
    }

    public void AddGold(int amount)
    {
        if (amount <= 0)
            return;

        Gold += amount;
        EventBus.Publish(new GoldChangedEvent(Gold));
    }
}
