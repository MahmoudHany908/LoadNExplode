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
        {
            EventBus.Publish(new BuyItemFailedEvent(evt.Item, BuyItemFailReason.InvalidRequest));
            Debug.LogWarning("[Shop] Buy failed: missing item, GoldManager, or PlayerInventory.");
            return;
        }

        if (!playerInventory.HasFreeSlot)
        {
            EventBus.Publish(new BuyItemFailedEvent(evt.Item, BuyItemFailReason.InventoryFull));
            Debug.LogWarning("[Shop] Buy failed: inventory is full (max 2 items).");
            return;
        }

        if (!goldManager.TrySpend(evt.Item.Price))
        {
            EventBus.Publish(new BuyItemFailedEvent(evt.Item, BuyItemFailReason.NotEnoughGold));
            Debug.LogWarning($"[Shop] Buy failed: not enough gold for '{evt.Item.ItemName}' (need {evt.Item.Price}).");
            return;
        }

        if (!playerInventory.TryAddItem(evt.Item, out int slotIndex))
        {
            // Should be rare: slot check passed but add failed. Refund gold.
            goldManager.AddGold(evt.Item.Price);
            EventBus.Publish(new BuyItemFailedEvent(evt.Item, BuyItemFailReason.InventoryFull));
            return;
        }

        EventBus.Publish(new BuyItemSucceededEvent(evt.Item, slotIndex));
        Debug.Log($"[Shop] Bought '{evt.Item.ItemName}' → inventory slot {slotIndex + 1}");
    }
}
