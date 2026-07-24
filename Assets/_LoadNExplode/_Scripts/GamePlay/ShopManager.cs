using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GoldManager goldManager;
    [SerializeField] private PlayerInventory playerInventory;

    private void Awake()
    {
        if (goldManager == null)
            goldManager = FindFirstObjectByType<GoldManager>();

        if (playerInventory == null)
            playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<BuyItemRequestedEvent>(OnBuyItemRequested);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<BuyItemRequestedEvent>(OnBuyItemRequested);
    }

    private void OnBuyItemRequested(BuyItemRequestedEvent evt)
    {
        if (evt.Item == null || goldManager == null || playerInventory == null)
            return;

        if (!playerInventory.HasFreeSlot)
            return;

        if (!goldManager.TrySpend(evt.Item.Price))
            return;

        playerInventory.TryAddItem(evt.Item);
    }
}
