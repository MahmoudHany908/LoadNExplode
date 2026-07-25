using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    private const int SlotCount = 2;

    // Keys corresponding to slots below. Index 0 pairs with slot 0, etc.
    // Mirrors AbilitiesLoadout.AbilityKeys.
    [SerializeField] private Key[] slotKeys = new Key[SlotCount] { Key.Digit1, Key.Digit2 };

    private readonly ShopItemDefinition[] _itemDefinitions = new ShopItemDefinition[SlotCount];
    private readonly IItem[] _activeItems = new IItem[SlotCount];
    private readonly List<InputAction> _inputActions = new List<InputAction>();

    private Player _player;

    public int Count
    {
        get
        {
            int count = 0;
            for (int i = 0; i < _itemDefinitions.Length; i++)
            {
                if (_itemDefinitions[i] != null)
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

    // Mirrors GamePlayBootstrapper.InitializeAbilities()'s InputAction wiring.
    private void InitializeInputActions()
    {
        int count = Mathf.Min(slotKeys.Length, SlotCount);

        for (int i = 0; i < count; i++)
        {
            int slotIndex = i; // capture for closure
            string keyName = slotKeys[i].ToString().ToLower();
            string bindingPath = $"<Keyboard>/{keyName}";

            InputAction action = new InputAction(binding: bindingPath);

            action.performed += ctx => TryUseItem(slotIndex);

            action.Enable();
            _inputActions.Add(action);
        }
    }

    private void Update()
    {
        // Mirrors GamePlayBootstrapper.Update() ticking active abilities.
        // Items are consume-on-use and can self-destroy (mine explodes, shield
        // expires, etc). Unity's "fake null" means a plain C# null-check on the
        // IItem reference isn't enough once the underlying GameObject is gone,
        // so we check the Unity Object identity explicitly and clear the slot.
        for (int i = 0; i < _activeItems.Length; i++)
        {
            IItem active = _activeItems[i];
            if (active == null)
                continue;

            if (active is Object unityObj && unityObj == null)
            {
                _activeItems[i] = null;
                continue;
            }

            active.Tick(Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        // Mirrors GamePlayBootstrapper.OnDestroy() cleanup.
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

    public ShopItemDefinition GetItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _itemDefinitions.Length)
            return null;

        return _itemDefinitions[slotIndex];
    }

    public bool TryAddItem(ShopItemDefinition item, out int slotIndex)
    {
        slotIndex = -1;

        if (item == null)
            return false;

        for (int i = 0; i < _itemDefinitions.Length; i++)
        {
            if (_itemDefinitions[i] != null)
                continue;

            _itemDefinitions[i] = item;
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

        GameObject instance = item.SpawnBehavior != null
            ? item.SpawnBehavior.Spawn(item.ItemPrefab, context)
            : Object.Instantiate(item.ItemPrefab, context.PlayerPosition, Quaternion.identity);

        // Mirrors GamePlayBootstrapper: fetch IItem, call Started() then Activate().
        IItem iitem = instance != null ? instance.GetComponent<IItem>() : null;
        if (iitem != null)
        {
            // Hand the player reference down directly so items don't need to
            // search the scene for it themselves.
            if (iitem is IPlayerReceivable playerReceivable)
                playerReceivable.SetPlayer(_player);

            _activeItems[slotIndex] = iitem;
            iitem.Started();
            iitem.Activate();
        }
        else
        {
            Debug.LogWarning($"Item prefab '{item.ItemPrefab.name}' does not have an IItem component.");
        }

        _itemDefinitions[slotIndex] = null;
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