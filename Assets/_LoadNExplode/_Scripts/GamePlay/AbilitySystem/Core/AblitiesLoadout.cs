using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Abilities/Ability Loadout")]
public class AbilitiesLoadout : ScriptableObject
{
    [Header("Abilities")]
    [Tooltip("List of Ability Prefabs. Index 0 pairs with Key Index 0, etc.")]
    public GameObject[] Abilities = new GameObject[3];

    [Header("Input Keys (New Input System)")]
    [Tooltip("Keys corresponding to the abilities above.")]
    public Key[] AbilityKeys = new Key[3];
}