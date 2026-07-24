using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private int startingGold;

    public int Gold { get; private set; }

    private void Awake()
    {
        Gold = startingGold;
    }

    private void Start()
    {
        EventBus.Publish(new GoldChangedEvent(Gold));
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
