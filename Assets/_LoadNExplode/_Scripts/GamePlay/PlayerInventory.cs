using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private const int SlotCount = 2;

    private readonly ShopItemDefinition[] _items = new ShopItemDefinition[SlotCount];

    public int Count
    {
        get
        {
            int count = 0;
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] != null)
                    count++;
            }

            return count;
        }
    }

    public int Capacity => SlotCount;

    public bool HasFreeSlot => Count < Capacity;

    public ShopItemDefinition GetItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _items.Length)
            return null;

        return _items[slotIndex];
    }

    public bool TryAddItem(ShopItemDefinition item)
    {
        if (item == null)
            return false;

        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] != null)
                continue;

            _items[i] = item;
            EventBus.Publish(new InventoryChangedEvent(this));
            return true;
        }

        return false;
    }

    public bool TryUseItem(int slotIndex)
    {
        ShopItemDefinition item = GetItem(slotIndex);
        if (item == null)
            return false;

        if (item.ItemPrefab != null)
            Instantiate(item.ItemPrefab, transform.position, transform.rotation);

        _items[slotIndex] = null;
        EventBus.Publish(new InventoryChangedEvent(this));
        return true;
    }
}
