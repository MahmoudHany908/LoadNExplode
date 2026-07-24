public struct BuyItemRequestedEvent : IGameEvent
{
    public readonly ShopItemDefinition Item;

    public BuyItemRequestedEvent(ShopItemDefinition item)
    {
        Item = item;
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
