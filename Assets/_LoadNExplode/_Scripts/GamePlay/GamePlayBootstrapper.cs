using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GamePlayBootstrapper : MonoBehaviour
{
    [Header("Abilities")]
    [SerializeField] private RectTransform abilitySlotParent;
    [SerializeField] private AbilitiesLoadout abilitiesLoadout;

    private List<IAbility> activeAbilities = new List<IAbility>();
    private List<InputAction> inputActions = new List<InputAction>();

    private void Awake()
    {
        InitializeAbilities();
    }

    private void InitializeAbilities()
    {
        if (abilitySlotParent == null)
        {
            Debug.LogError("Ability Slot Parent is not assigned in the inspector.");
            return;
        }

        if (abilitiesLoadout == null)
        {
            Debug.LogError("Abilities Loadout is not assigned in the inspector.");
            return;
        }

        if (abilitiesLoadout.Abilities == null || abilitiesLoadout.AbilityKeys == null)
        {
            Debug.LogError("Abilities or AbilityKeys arrays are null in the Loadout.");
            return;
        }

        // Safety check: Only process up to the length of the shortest array 
        // to prevent IndexOutOfRangeException if they get mismatched.
        int count = Mathf.Min(abilitiesLoadout.Abilities.Length, abilitiesLoadout.AbilityKeys.Length);

        for (int i = 0; i < count; i++)
        {
            GameObject abilityPrefab = abilitiesLoadout.Abilities[i];
            Key triggerKey = abilitiesLoadout.AbilityKeys[i];

            if (abilityPrefab == null)
            {
                Debug.LogWarning($"Ability prefab at index {i} is null. Skipping.");
                continue;
            }

            GameObject abilityInstance = Instantiate(abilityPrefab, abilitySlotParent);

            IAbility ability = abilityInstance.GetComponent<IAbility>();

            if (ability == null)
            {
                Debug.LogWarning($"Prefab '{abilityPrefab.name}' at index {i} does not have an IAbility component. Skipping.");
                Destroy(abilityInstance);
                continue;
            }

            activeAbilities.Add(ability);
            ability.Started();

            string keyName = triggerKey.ToString().ToLower();
            string bindingPath = $"<Keyboard>/{keyName}";

            InputAction action = new InputAction(binding: bindingPath);


            action.performed += ctx =>
            {
                if (ability != null)
                {
                    ability.Activate();
                }
            };

            action.Enable();
            inputActions.Add(action);
        }
    }

    private void Update()
    {

        foreach (var ability in activeAbilities)
        {
            ability?.Tick(Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        // Clean up dynamically created InputActions to prevent memory leaks
        foreach (var action in inputActions)
        {
            if (action != null)
            {
                action.Disable();
                action.Dispose();
            }
        }

        inputActions.Clear();
        activeAbilities.Clear();
    }
}