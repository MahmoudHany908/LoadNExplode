using UnityEngine;

public struct BuyItemRequestedEvent : IGameEvent
{
    public readonly ItemMetadata Item;

    public BuyItemRequestedEvent(ItemMetadata item)
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
    public readonly ItemMetadata Item;
    public readonly BuyItemFailReason Reason;

    public BuyItemFailedEvent(ItemMetadata item, BuyItemFailReason reason)
    {
        Item = item;
        Reason = reason;
    }
}

public struct BuyItemSucceededEvent : IGameEvent
{
    public readonly ItemMetadata Item;
    public readonly int SlotIndex;

    public BuyItemSucceededEvent(ItemMetadata item, int slotIndex)
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
