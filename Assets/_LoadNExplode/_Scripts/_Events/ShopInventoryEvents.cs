public struct BuyItemRequestedEvent : IGameEvent
{
    public readonly ShopItemDefinition Item;

    public BuyItemRequestedEvent(ShopItemDefinition item)
    {
        Item = item;
    }
}

public enum BuyItemFailReason
{
    InventoryFull,
    NotEnoughGold,
    InvalidRequest
}

public struct BuyItemFailedEvent : IGameEvent
{
    public readonly ShopItemDefinition Item;
    public readonly BuyItemFailReason Reason;

    public BuyItemFailedEvent(ShopItemDefinition item, BuyItemFailReason reason)
    {
        Item = item;
        Reason = reason;
    }
}

public struct BuyItemSucceededEvent : IGameEvent
{
    public readonly ShopItemDefinition Item;
    public readonly int SlotIndex;

    public BuyItemSucceededEvent(ShopItemDefinition item, int slotIndex)
    {
        Item = item;
        SlotIndex = slotIndex;
    }
}

public struct InventoryChangedEvent : IGameEvent
{
    public readonly PlayerInventory Inventory;

    public InventoryChangedEvent(PlayerInventory inventory)
    {
        Inventory = inventory;
    }
}

public struct GoldChangedEvent : IGameEvent
{
    public readonly int Gold;

    public GoldChangedEvent(int gold)
    {
        Gold = gold;
    }
}
