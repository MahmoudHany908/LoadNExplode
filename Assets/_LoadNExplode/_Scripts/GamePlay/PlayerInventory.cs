using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private const int SlotCount = 2;

    [SerializeField] private RectTransform itemSlotParent;
    [SerializeField] private Key[] slotKeys = new Key[SlotCount] { Key.Digit1, Key.Digit2 };

    private readonly GameObject[] _inventorySlots = new GameObject[SlotCount];
    private readonly IItem[] _activeItems = new IItem[SlotCount];
    private readonly List<InputAction> _inputActions = new List<InputAction>();

    private Player _player;

    public int Count
    {
        get
        {
            int count = 0;
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                if (_inventorySlots[i] != null)
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
        InitializeInputActions();
    }

    private void InitializeInputActions()
    {
        // Explicitly use the correct path format for numbers
        string[] paths = { "<Keyboard>/1", "<Keyboard>/2" };
        
        int count = Mathf.Min(paths.Length, SlotCount);

        for (int i = 0; i < count; i++)
        {
            int slotIndex = i; // capture for closure
            string bindingPath = paths[i];

            InputAction action = new InputAction(binding: bindingPath);

            action.performed += ctx => TryUseItem(slotIndex);

            action.Enable();
            _inputActions.Add(action);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _activeItems.Length; i++)
        {
            IItem active = _activeItems[i];
            if (active == null)
                continue;

            if (active is Object unityObj && unityObj == null)
            {
                _activeItems[i] = null;
                _inventorySlots[i] = null; // Also clear the slot reference if the object is destroyed
                continue;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var action in _inputActions)
        {
            if (action != null)
            {
                action.Disable();
                action.Dispose();
            }
        }
        _inputActions.Clear();
    }

    public bool TryAddItem(GameObject itemPrefab, out int slotIndex)
    {
        slotIndex = -1;

        if (itemPrefab == null)
            return false;

        if (itemSlotParent == null)
        {
            Debug.LogWarning("ItemSlotParent is not assigned in PlayerInventory!");
            return false;
        }

        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (_inventorySlots[i] != null)
                continue;

            GameObject instance = Instantiate(itemPrefab, itemSlotParent);
            _inventorySlots[i] = instance;
            
            IItem iitem = instance.GetComponent<IItem>();
            if (iitem != null)
            {
                if (iitem is IPlayerReceivable playerReceivable)
                    playerReceivable.SetPlayer(_player);

                _activeItems[i] = iitem;
            }
            else
            {
                Debug.LogWarning($"Item prefab '{itemPrefab.name}' does not have an IItem component.");
            }

            // Optional: if the prefab has a UI button, hook it up so clicking works like hotkeys
            UnityEngine.UI.Button btn = instance.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                int capturedIndex = i;
                btn.onClick.AddListener(() => TryUseItem(capturedIndex));
            }

            slotIndex = i;
            EventBus.Publish(new InventoryChangedEvent(this));
            return true;
        }

        return false;
    }

    public bool TryAddItem(GameObject itemPrefab)
    {
        return TryAddItem(itemPrefab, out _);
    }

    public bool TryUseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _inventorySlots.Length)
            return false;

        IItem item = _activeItems[slotIndex];
        if (item == null)
            return false;

        item.Activate();

        GameObject itemGo = _inventorySlots[slotIndex];
        if (itemGo != null)
        {
            itemGo.transform.SetParent(null);
            UnityEngine.UI.Image img = itemGo.GetComponent<UnityEngine.UI.Image>();
            if (img != null) img.enabled = false;
        }

        // Clear the used slot
        _inventorySlots[slotIndex] = null;
        _activeItems[slotIndex] = null;

        // Shift remaining items to the left so they match the UI hierarchy which automatically shifts
        for (int i = slotIndex; i < _inventorySlots.Length - 1; i++)
        {
            _inventorySlots[i] = _inventorySlots[i + 1];
            _inventorySlots[i + 1] = null;

            _activeItems[i] = _activeItems[i + 1];
            _activeItems[i + 1] = null;
        }

        EventBus.Publish(new InventoryChangedEvent(this));
        return true;
    }
}