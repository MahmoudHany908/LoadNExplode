using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private List<Image> slotIcons = new();
    [SerializeField] private List<Button> useButtons = new();

    private readonly List<UnityAction> _buttonListeners = new();

    private void Awake()
    {
        if (inventory == null)
            inventory = FindFirstObjectByType<PlayerInventory>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<InventoryChangedEvent>(OnInventoryChanged);

        for (int i = 0; i < useButtons.Count; i++)
        {
            int slotIndex = i;

            if (useButtons[i] == null)
                continue;

            UnityAction listener = () => UseSlot(slotIndex);
            _buttonListeners.Add(listener);
            useButtons[i].onClick.AddListener(listener);
        }

        Refresh();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<InventoryChangedEvent>(OnInventoryChanged);

        int count = Mathf.Min(useButtons.Count, _buttonListeners.Count);

        for (int i = 0; i < count; i++)
        {
            if (useButtons[i] != null)
                useButtons[i].onClick.RemoveListener(_buttonListeners[i]);
        }

        _buttonListeners.Clear();
    }

    private void OnInventoryChanged(InventoryChangedEvent evt)
    {
        inventory = evt.Inventory;
        Refresh();
    }

    private void Refresh()
    {
        if (inventory == null)
            return;

        for (int i = 0; i < slotIcons.Count; i++)
        {
            ShopItemDefinition item = inventory.GetItem(i);
            Image icon = slotIcons[i];

            if (icon == null)
                continue;

            icon.sprite = item != null ? item.Icon : null;
            icon.enabled = item != null && item.Icon != null;
        }
    }

    private void UseSlot(int slotIndex)
    {
        if (inventory != null)
            inventory.TryUseItem(slotIndex);
    }
}
