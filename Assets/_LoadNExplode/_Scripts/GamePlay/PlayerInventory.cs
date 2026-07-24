using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private const int SlotCount = 2;

    private readonly ShopItemDefinition[] _items = new ShopItemDefinition[SlotCount];
    private Player _player;

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

    private void Awake()
    {
        _player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        // Slot 0 = key 1, Slot 1 = key 2 (abilities use Z/X/C — no conflict)
        if (keyboard.digit1Key.wasPressedThisFrame)
            TryUseItem(0);

        if (keyboard.digit2Key.wasPressedThisFrame)
            TryUseItem(1);
    }

    public ShopItemDefinition GetItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _items.Length)
            return null;

        return _items[slotIndex];
    }

    public bool TryAddItem(ShopItemDefinition item, out int slotIndex)
    {
        slotIndex = -1;

        if (item == null)
            return false;

        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] != null)
                continue;

            _items[i] = item;
            slotIndex = i;
            EventBus.Publish(new InventoryChangedEvent(this));
            return true;
        }

        return false;
    }

    public bool TryAddItem(ShopItemDefinition item)
    {
        return TryAddItem(item, out _);
    }

    public bool TryUseItem(int slotIndex)
    {
        ShopItemDefinition item = GetItem(slotIndex);
        if (item == null || item.ItemPrefab == null)
            return false;

        if (_player == null)
            _player = FindFirstObjectByType<Player>();

        ItemUseContext context = BuildUseContext();

        if (item.SpawnBehavior != null)
            item.SpawnBehavior.Spawn(item.ItemPrefab, context);
        else
            Object.Instantiate(item.ItemPrefab, context.PlayerPosition, Quaternion.identity);

        _items[slotIndex] = null;
        EventBus.Publish(new InventoryChangedEvent(this));
        return true;
    }

    private ItemUseContext BuildUseContext()
    {
        Vector3 playerPos = _player != null ? _player.transform.position : transform.position;
        Vector3 aim = Vector3.forward;

        if (_player != null && PlayerInputs.Instance != null)
        {
            Vector3 mouseWorld = PlayerInputs.Instance.GetMouseWorldPosition();
            aim = mouseWorld - playerPos;
            aim.y = 0f;

            if (aim.sqrMagnitude < 0.0001f)
                aim = _player.transform.forward;
        }

        return new ItemUseContext(_player, playerPos, aim);
    }
}
